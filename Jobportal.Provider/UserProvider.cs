using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jobportal.Models;
using Microsoft.Extensions.Configuration;

namespace Jobportal.Provider
{
    public interface IUserProvider
    {
        object CreateProfile(JobSeekerProfile profile);
        (JobSeekerProfile Profile, int StatusCode, string Message) JobSeekerProfiles(int jobSeekerId);
        (bool IsSuccess, int StatusCode, string Message) UpdateProfile(int jobSeekerId, JobSeekerProfile profile);
        (List<Job> Jobs, int StatusCode, string Message) GetAllJobs();
        (Job Job, int StatusCode, string Message) GetJobById(int jobId);
        (bool IsSuccess, int StatusCode, string Message) SubmitApplication(JobApplication app);
        (List<ApplicationStatus> History, bool IsSuccess, int StatusCode, string Message) GetStatusHistory(int applicationId);
    }

    public class UserProvider : IUserProvider
    {
        private readonly string _connectionString;

        public UserProvider(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

       
        public object CreateProfile(JobSeekerProfile profile)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_jobportal_addjobseekerprofile", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Input parameters
                        cmd.Parameters.AddWithValue("@p_JobSeekerId", profile.JobSeekerId);
                        cmd.Parameters.AddWithValue("@p_DateOfBirth", profile.DateOfBirth);
                        cmd.Parameters.AddWithValue("@p_Gender", string.IsNullOrEmpty(profile.Gender) ? DBNull.Value : (object)profile.Gender);
                        cmd.Parameters.AddWithValue("@p_Address", string.IsNullOrEmpty(profile.Address) ? DBNull.Value : (object)profile.Address);
                        cmd.Parameters.AddWithValue("@p_Education", string.IsNullOrEmpty(profile.Education) ? DBNull.Value : (object)profile.Education);
                        cmd.Parameters.AddWithValue("@p_Experience", string.IsNullOrEmpty(profile.Experience) ? DBNull.Value : (object)profile.Experience);
                        cmd.Parameters.AddWithValue("@p_Skills", string.IsNullOrEmpty(profile.Skills) ? DBNull.Value : (object)profile.Skills);
                        cmd.Parameters.AddWithValue("@p_ProfilePhoto", string.IsNullOrEmpty(profile.ProfilePhoto) ? DBNull.Value : (object)profile.ProfilePhoto);
                        cmd.Parameters.AddWithValue("@p_CV", string.IsNullOrEmpty(profile.CV) ? DBNull.Value : (object)profile.CV);

                        // Output parameters
                        SqlParameter statusParam = new SqlParameter("@p_error_status_code", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(statusParam);

                        SqlParameter messageParam = new SqlParameter("@p_error_message", SqlDbType.NVarChar, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        con.Open();
                        cmd.ExecuteNonQuery();

                        return new
                        {
                            StatusCode = (int)(statusParam.Value ?? -1),
                            Message = messageParam.Value?.ToString() ?? "Unknown error"
                        };
                    }
                }
            }
            catch (SqlException ex)
            {
                
                return new
                {
                    StatusCode = -99,
                    Message = $"Database error: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    StatusCode = -98,
                    Message = $"Unexpected error: {ex.Message}"
                };
            }
        }


        
        public (JobSeekerProfile Profile, int StatusCode, string Message) JobSeekerProfiles(int jobSeekerId)
        {
            JobSeekerProfile profile = null;

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_jobportal_getjobseekerprofilebyid", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Input parameter
                        cmd.Parameters.AddWithValue("@p_JobSeekerId", jobSeekerId);

                        // Output parameters
                        SqlParameter statusCodeParam = new SqlParameter("@p_error_status_code", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(statusCodeParam);

                        SqlParameter messageParam = new SqlParameter("@p_error_message", SqlDbType.NVarChar, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                profile = new JobSeekerProfile
                                {
                                    ProfileId = reader["ProfileId"] != DBNull.Value ? Convert.ToInt32(reader["ProfileId"]) : 0,
                                    JobSeekerId = reader["JobSeekerId"] != DBNull.Value ? Convert.ToInt32(reader["JobSeekerId"]) : 0,
                                    DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime)reader["DateOfBirth"] : default(DateTime),
                                    Gender = reader["Gender"]?.ToString(),
                                    Address = reader["Address"]?.ToString(),
                                    Education = reader["Education"]?.ToString(),
                                    Experience = reader["Experience"]?.ToString(),
                                    Skills = reader["Skills"]?.ToString(),
                                    ProfilePhoto = reader["ProfilePhoto"]?.ToString(),
                                    CV = reader["CV"]?.ToString()
                                };
                            }
                        }

                        return (
                            Profile: profile,
                            StatusCode: statusCodeParam.Value != DBNull.Value ? (int)statusCodeParam.Value : -1,
                            Message: messageParam.Value?.ToString() ?? "Unknown error"
                        );
                    }
                }
            }
            catch (SqlException ex)
            {
                return (
                    Profile: null,
                    StatusCode: -99,
                    Message: "Database error: " + ex.Message
                );
            }
            catch (Exception ex)
            {
                return (
                    Profile: null,
                    StatusCode: -98,
                    Message: "Unexpected error: " + ex.Message
                );
            }
        }


       
        public (bool IsSuccess, int StatusCode, string Message) UpdateProfile(int jobSeekerId, JobSeekerProfile profile)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_jobportal_updatejobseekerprofile", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Input Parameters
                        cmd.Parameters.AddWithValue("@p_JobSeekerId", jobSeekerId);
                        cmd.Parameters.AddWithValue("@p_DateOfBirth", profile.DateOfBirth);
                        cmd.Parameters.AddWithValue("@p_Gender", profile.Gender ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_Address", profile.Address ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_Education", profile.Education ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_Experience", profile.Experience ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_Skills", profile.Skills ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_ProfilePhoto", profile.ProfilePhoto ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_CV", profile.CV ?? (object)DBNull.Value);

                        // Output Parameters
                        SqlParameter statusCodeParam = new SqlParameter("@p_error_status_code", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(statusCodeParam);

                        SqlParameter messageParam = new SqlParameter("@p_error_message", SqlDbType.NVarChar, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        con.Open();
                        cmd.ExecuteNonQuery();

                        return (
                            IsSuccess: (int)statusCodeParam.Value == 1,
                            StatusCode: (int)statusCodeParam.Value,
                            Message: messageParam.Value?.ToString() ?? "Unknown status"
                        );
                    }
                }
            }
            catch (SqlException ex)
            {
                return (
                    IsSuccess: false,
                    StatusCode: -99,
                    Message: "SQL error: " + ex.Message
                );
            }
            catch (Exception ex)
            {
                return (
                    IsSuccess: false,
                    StatusCode: -98,
                    Message: "Unhandled error: " + ex.Message
                );
            }
        }


        
        public (List<Job> Jobs, int StatusCode, string Message) GetAllJobs()
        {
            List<Job> jobs = new List<Job>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_jobportal_getalljobs", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Output Parameters
                        SqlParameter statusCodeParam = new SqlParameter("@p_error_status_code", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(statusCodeParam);

                        SqlParameter messageParam = new SqlParameter("@p_error_message", SqlDbType.NVarChar, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            jobs.Add(new Job
                            {
                                JobId = Convert.ToInt32(reader["JobId"]),
                                RecruiterId = Convert.ToInt32(reader["RecruiterId"]),
                                JobTitle = reader["JobTitle"].ToString(),
                                CompanyName = reader["CompanyName"].ToString(),
                                Location = reader["Location"].ToString(),
                                EmploymentType = reader["EmploymentType"].ToString(),
                                Description = reader["Description"].ToString(),
                                Requirements = reader["Requirements"].ToString(),
                                PostedDate = Convert.ToDateTime(reader["PostedDate"]),
                                ApplicationDeadline = Convert.ToDateTime(reader["ApplicationDeadline"])
                            });
                        }

                        reader.Close(); 

                        return (
                            Jobs: jobs,
                            StatusCode: (int)statusCodeParam.Value,
                            Message: messageParam.Value?.ToString() ?? "Unknown status"
                        );
                    }
                }
            }
            catch (SqlException ex)
            {
                return (
                    Jobs: new List<Job>(),
                    StatusCode: -99,
                    Message: "SQL error: " + ex.Message
                );
            }
            catch (Exception ex)
            {
                return (
                    Jobs: new List<Job>(),
                    StatusCode: -98,
                    Message: "Unhandled error: " + ex.Message
                );
            }
        }


       
        public (Job Job, int StatusCode, string Message) GetJobById(int jobId)
        {
            Job job = null;

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_jobportal_getjobbyid", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Input parameter
                        cmd.Parameters.AddWithValue("@p_JobId", jobId);

                        // Output parameters
                        SqlParameter statusCodeParam = new SqlParameter("@p_error_status_code", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(statusCodeParam);

                        SqlParameter messageParam = new SqlParameter("@p_error_message", SqlDbType.NVarChar, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            job = new Job
                            {
                                JobId = Convert.ToInt32(reader["JobId"]),
                                RecruiterId = Convert.ToInt32(reader["RecruiterId"]),
                                JobTitle = reader["JobTitle"].ToString(),
                                CompanyName = reader["CompanyName"].ToString(),
                                Location = reader["Location"].ToString(),
                                EmploymentType = reader["EmploymentType"].ToString(),
                                Description = reader["Description"].ToString(),
                                Requirements = reader["Requirements"].ToString(),
                                PostedDate = Convert.ToDateTime(reader["PostedDate"]),
                                ApplicationDeadline = Convert.ToDateTime(reader["ApplicationDeadline"])
                            };
                        }

                        reader.Close();

                        return (
                            Job: job,
                            StatusCode: (int)statusCodeParam.Value,
                            Message: messageParam.Value?.ToString() ?? "No message returned"
                        );
                    }
                }
            }
            catch (SqlException ex)
            {
                return (
                    Job: null,
                    StatusCode: -99,
                    Message: "SQL error: " + ex.Message
                );
            }
            catch (Exception ex)
            {
                return (
                    Job: null,
                    StatusCode: -98,
                    Message: "Unhandled error: " + ex.Message
                );
            }
        }

        
        public (bool IsSuccess, int StatusCode, string Message) SubmitApplication(JobApplication app)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_jobportal_submitjobapplication", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Input Parameters
                        cmd.Parameters.AddWithValue("@p_JobSeekerId", app.JobSeekerId);
                        cmd.Parameters.AddWithValue("@p_JobId", app.JobId);
                        cmd.Parameters.AddWithValue("@p_ResumeUrl", app.ResumeUrl ?? string.Empty);
                        cmd.Parameters.AddWithValue("@p_CoverLetter", app.CoverLetter ?? string.Empty);
                        cmd.Parameters.AddWithValue("@p_AppliedDate", app.AppliedDate);
                        cmd.Parameters.AddWithValue("@p_CurrentStatus", app.CurrentStatus ?? string.Empty);
                        cmd.Parameters.AddWithValue("@p_Education", app.Education ?? string.Empty);
                        cmd.Parameters.AddWithValue("@p_Experience", app.Experience ?? string.Empty);
                        cmd.Parameters.AddWithValue("@p_Skills", app.Skills ?? string.Empty);
                        cmd.Parameters.AddWithValue("@p_ProfileSummary", app.ProfileSummary ?? string.Empty);
                        cmd.Parameters.AddWithValue("@p_ProfilePictureUrl", app.ProfilePictureUrl ?? string.Empty);
                        cmd.Parameters.AddWithValue("@p_Mobile", app.Mobile ?? string.Empty);
                        cmd.Parameters.AddWithValue("@p_Gender", app.Gender ?? string.Empty);
                        cmd.Parameters.AddWithValue("@p_DateOfBirth", app.DateOfBirth);

                        // Output Parameters
                        SqlParameter statusCodeParam = new SqlParameter("@p_error_status_code", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(statusCodeParam);

                        SqlParameter messageParam = new SqlParameter("@p_error_message", SqlDbType.NVarChar, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        con.Open();
                        cmd.ExecuteNonQuery();

                        int statusCode = (int)statusCodeParam.Value;
                        string message = messageParam.Value?.ToString() ?? "No message returned";

                        return (statusCode == 1, statusCode, message);
                    }
                }
            }
            catch (SqlException ex)
            {
                return (false, -99, "SQL error: " + ex.Message);
            }
            catch (Exception ex)
            {
                return (false, -98, "Unhandled error: " + ex.Message);
            }
        }

        public (List<ApplicationStatus> History, bool IsSuccess, int StatusCode, string Message) GetStatusHistory(int applicationId)
        {
            List<ApplicationStatus> history = new List<ApplicationStatus>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_jobportal_getstatushistory", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_ApplicationId", applicationId);

                    SqlParameter statusCode = new SqlParameter("@p_error_status_code", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    SqlParameter errorMessage = new SqlParameter("@p_error_message", SqlDbType.NVarChar, 4000)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(statusCode);
                    cmd.Parameters.Add(errorMessage);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            history.Add(new ApplicationStatus
                            {
                                ApplicationId = reader["ApplicationId"] != DBNull.Value ? Convert.ToInt32(reader["ApplicationId"]) : 0,
                                Status = reader["Status"]?.ToString(),
                                StatusUpdatedAt = reader["StatusUpdatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["StatusUpdatedAt"]) : DateTime.MinValue,
                                Remarks = reader["Remarks"]?.ToString()
                            });
                        }
                    }

                    int code = statusCode.Value != DBNull.Value ? (int)statusCode.Value : -99;
                    string msg = errorMessage.Value?.ToString();

                    return (history, true, code, msg);
                }
            }
            catch (SqlException ex)
            {
                return (null, false, -99, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (null, false, -98, $"Unhandled error: {ex.Message}");
            }
        }


    }
}
    

    