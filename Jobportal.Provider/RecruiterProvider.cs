using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Jobportal.Models;
using Microsoft.Extensions.Configuration;


namespace Jobportal.Provider
{
    public interface IRecruiterProvider
    {
        (bool Success, string Message) CreateRecruiterProfile(RecruiterProfile profile);
        (RecruiterProfile Profile, string Message) GetRecruiterProfile(int recruiterId);
        bool UpdateRecruiterProfile(RecruiterProfile profile);
        object AddJob(Job job);
        bool DeleteJob(int jobId);
        List<JobApplication> GetApplicationsByJobSeeker(int jobSeekerId);
        JobApplication GetApplicationById(int applicationId);
        bool AddStatus(ApplicationStatus status);
        bool UpdateStatus(ApplicationStatus status);
    }

    public class RecruiterProvider : IRecruiterProvider
    {
        private readonly string _connectionString;

        public RecruiterProvider(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
       
        public (bool Success, string Message) CreateRecruiterProfile(RecruiterProfile profile)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("sp_jobportal_addrecruiterprofile", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Input parameters
                cmd.Parameters.AddWithValue("@p_RecruiterId", profile.RecruiterId);
                cmd.Parameters.AddWithValue("@p_CompanyName", profile.CompanyName);
                cmd.Parameters.AddWithValue("@p_RecruiterName", profile.RecruiterName);
                cmd.Parameters.AddWithValue("@p_Email", profile.Email);
                cmd.Parameters.AddWithValue("@p_Contact", profile.Contact);
                cmd.Parameters.AddWithValue("@p_Designation", profile.Designation);
                cmd.Parameters.AddWithValue("@p_Location", profile.Location);
                cmd.Parameters.AddWithValue("@p_AboutCompany", profile.AboutCompany);

                // Output parameters
                SqlParameter statusCodeParam = new SqlParameter("@p_ErrorStatusCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                SqlParameter messageParam = new SqlParameter("@p_ErrorMessage", SqlDbType.NVarChar, 4000)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(statusCodeParam);
                cmd.Parameters.Add(messageParam);

                // Execute the procedure
                con.Open();
                cmd.ExecuteNonQuery();

                int statusCode = (int)statusCodeParam.Value;
                string message = messageParam.Value?.ToString();

                return (statusCode == 1, message);
            }
            catch (Exception ex)
            {
              
                return (false, "An unexpected error occurred: " + ex.Message);
            }
        }



        
        public (RecruiterProfile Profile, string Message) GetRecruiterProfile(int recruiterId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("sp_jobportal_getrecruiterprofile", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Input parameter
                cmd.Parameters.AddWithValue("@p_RecruiterId", recruiterId);

                // Output parameters
                SqlParameter profileIdParam = new SqlParameter("@p_ProfileId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                SqlParameter companyNameParam = new SqlParameter("@p_CompanyName", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output };
                SqlParameter recruiterNameParam = new SqlParameter("@p_RecruiterName", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output };
                SqlParameter emailParam = new SqlParameter("@p_Email", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output };
                SqlParameter contactParam = new SqlParameter("@p_Contact", SqlDbType.NVarChar, 20) { Direction = ParameterDirection.Output };
                SqlParameter designationParam = new SqlParameter("@p_Designation", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output };
                SqlParameter locationParam = new SqlParameter("@p_Location", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output };
                SqlParameter aboutCompanyParam = new SqlParameter("@p_AboutCompany", SqlDbType.NVarChar, 1000) { Direction = ParameterDirection.Output };
                SqlParameter statusCodeParam = new SqlParameter("@p_ErrorStatusCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                SqlParameter messageParam = new SqlParameter("@p_ErrorMessage", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };

                // Add output parameters to command
                cmd.Parameters.Add(profileIdParam);
                cmd.Parameters.Add(companyNameParam);
                cmd.Parameters.Add(recruiterNameParam);
                cmd.Parameters.Add(emailParam);
                cmd.Parameters.Add(contactParam);
                cmd.Parameters.Add(designationParam);
                cmd.Parameters.Add(locationParam);
                cmd.Parameters.Add(aboutCompanyParam);
                cmd.Parameters.Add(statusCodeParam);
                cmd.Parameters.Add(messageParam);

                con.Open();
                cmd.ExecuteNonQuery();

                int statusCode = (int)(statusCodeParam.Value ?? -1);
                string message = messageParam.Value?.ToString();

                if (statusCode == 1)
                {
                    var profile = new RecruiterProfile
                    {
                        ProfileId = Convert.ToInt32(profileIdParam.Value),
                        RecruiterId = recruiterId,
                        CompanyName = companyNameParam.Value?.ToString(),
                        RecruiterName = recruiterNameParam.Value?.ToString(),
                        Email = emailParam.Value?.ToString(),
                        Contact = contactParam.Value?.ToString(),
                        Designation = designationParam.Value?.ToString(),
                        Location = locationParam.Value?.ToString(),
                        AboutCompany = aboutCompanyParam.Value?.ToString()
                    };

                    return (profile, message);
                }

                return (null, message);
            }
            catch (Exception ex)
            {
                return (null, "An unexpected error occurred: " + ex.Message);
            }
        }


        
        public bool UpdateRecruiterProfile(RecruiterProfile profile)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("sp_jobportal_updaterecruiterprofile", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@p_RecruiterId", profile.RecruiterId);
                cmd.Parameters.AddWithValue("@p_CompanyName", profile.CompanyName);
                cmd.Parameters.AddWithValue("@p_RecruiterName", profile.RecruiterName);
                cmd.Parameters.AddWithValue("@p_Email", profile.Email);
                cmd.Parameters.AddWithValue("@p_Contact", profile.Contact);
                cmd.Parameters.AddWithValue("@p_Designation", profile.Designation);
                cmd.Parameters.AddWithValue("@p_Location", profile.Location);
                cmd.Parameters.AddWithValue("@p_AboutCompany", profile.AboutCompany);

                cmd.Parameters.Add("@p_ErrorStatusCode", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@p_ErrorMessage", SqlDbType.NVarChar, 4000).Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();

                int statusCode = Convert.ToInt32(cmd.Parameters["@p_ErrorStatusCode"].Value);
                string message = cmd.Parameters["@p_ErrorMessage"].Value.ToString();

                Console.WriteLine($"Update Recruiter Profile - StatusCode: {statusCode}, Message: {message}");

                return statusCode == 1;
            }
            catch (SqlException sqlEx)
            {
                
                Console.WriteLine($"SQL Exception in UpdateRecruiterProfile: {sqlEx.Message}");
                
                return false;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Exception in UpdateRecruiterProfile: {ex.Message}");
                
                return false;
            }
        }

        
        public object AddJob(Job job)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("sp_jobportal_addjob", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@p_RecruiterId", job.RecruiterId);
                cmd.Parameters.AddWithValue("@p_JobTitle", job.JobTitle);
                cmd.Parameters.AddWithValue("@p_CompanyName", job.CompanyName);
                cmd.Parameters.AddWithValue("@p_Location", job.Location);
                cmd.Parameters.AddWithValue("@p_EmploymentType", job.EmploymentType);
                cmd.Parameters.AddWithValue("@p_Description", job.Description);
                cmd.Parameters.AddWithValue("@p_Requirements", job.Requirements);
                cmd.Parameters.AddWithValue("@p_ApplicationDeadline", job.ApplicationDeadline);

                cmd.Parameters.Add("@p_JobId", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@p_ErrorStatusCode", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@p_ErrorMessage", SqlDbType.NVarChar, 4000).Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();

                int statusCode = Convert.ToInt32(cmd.Parameters["@p_ErrorStatusCode"].Value);
                string message = cmd.Parameters["@p_ErrorMessage"].Value.ToString();
                int jobId = Convert.ToInt32(cmd.Parameters["@p_JobId"].Value);

                return new
                {
                    Success = statusCode == 1,
                    JobId = jobId,
                    Message = message
                };
            }
            catch (SqlException sqlEx)
            {
                
                Console.WriteLine($"SQL Exception in AddJob: {sqlEx.Message}");
                return new
                {
                    Success = false,
                    JobId = 0,
                    Message = "A database error occurred: " + sqlEx.Message
                };
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Exception in AddJob: {ex.Message}");
                return new
                {
                    Success = false,
                    JobId = 0,
                    Message = "An unexpected error occurred: " + ex.Message
                };
            }
        }

       
        public bool DeleteJob(int jobId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("sp_jobportal_deletejob", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@p_JobId", jobId);
                cmd.Parameters.Add("@p_ErrorStatusCode", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@p_ErrorMessage", SqlDbType.NVarChar, 4000).Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();

                int statusCode = Convert.ToInt32(cmd.Parameters["@p_ErrorStatusCode"].Value);
                string message = cmd.Parameters["@p_ErrorMessage"].Value.ToString();

                return statusCode == 1;
            }
            catch (SqlException sqlEx)
            {
                
                Console.WriteLine($"SQL Exception in DeleteJob: {sqlEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Log general exception details here
                Console.WriteLine($"Exception in DeleteJob: {ex.Message}");
                return false;
            }
        }

        
        public List<JobApplication> GetApplicationsByJobSeeker(int jobSeekerId)
        {
            List<JobApplication> applications = new List<JobApplication>();

            try
            {
                // Open SQL connection
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_jobportal_getapplicationsbyjobseeker", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add input parameter for job seeker ID
                        cmd.Parameters.AddWithValue("@p_JobSeekerId", jobSeekerId);
                        cmd.Parameters.Add("@p_ErrorStatusCode", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@p_ErrorMessage", SqlDbType.NVarChar, 4000).Direction = ParameterDirection.Output;

                        // Add output parameters for the job application details
                        cmd.Parameters.Add("@p_ApplicationId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@p_JobId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@p_ResumeUrl", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@p_CoverLetter", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@p_AppliedDate", SqlDbType.DateTime).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@p_CurrentStatus", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@p_Education", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@p_Experience", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@p_Skills", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@p_ProfileSummary", SqlDbType.NVarChar, 1000).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@p_ProfilePictureUrl", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@p_Mobile", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@p_Gender", SqlDbType.NVarChar, 10).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@p_DateOfBirth", SqlDbType.DateTime).Direction = ParameterDirection.Output;

                        // Open connection to the database
                        con.Open();

                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                
                                JobApplication application = new JobApplication
                                {
                                    ApplicationId = Convert.ToInt32(cmd.Parameters["@p_ApplicationId"].Value),
                                    JobId = Convert.ToInt32(cmd.Parameters["@p_JobId"].Value),
                                    ResumeUrl = cmd.Parameters["@p_ResumeUrl"].Value.ToString(),
                                    CoverLetter = cmd.Parameters["@p_CoverLetter"].Value.ToString(),
                                    AppliedDate = Convert.ToDateTime(cmd.Parameters["@p_AppliedDate"].Value),
                                    CurrentStatus = cmd.Parameters["@p_CurrentStatus"].Value.ToString(),
                                    Education = cmd.Parameters["@p_Education"].Value.ToString(),
                                    Experience = cmd.Parameters["@p_Experience"].Value.ToString(),
                                    Skills = cmd.Parameters["@p_Skills"].Value.ToString(),
                                    ProfileSummary = cmd.Parameters["@p_ProfileSummary"].Value.ToString(),
                                    ProfilePictureUrl = cmd.Parameters["@p_ProfilePictureUrl"].Value.ToString(),
                                    Mobile = cmd.Parameters["@p_Mobile"].Value.ToString(),
                                    Gender = cmd.Parameters["@p_Gender"].Value.ToString(),
                                    DateOfBirth = Convert.ToDateTime(cmd.Parameters["@p_DateOfBirth"].Value)
                                };

                               
                                applications.Add(application);
                            }
                        }

                       
                        int statusCode = Convert.ToInt32(cmd.Parameters["@p_ErrorStatusCode"].Value);
                        string message = cmd.Parameters["@p_ErrorMessage"].Value.ToString();

                        if (statusCode != 1)
                        {
                            throw new Exception($"Error fetching job applications: {message}");
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Handle SQL specific exceptions
                throw new Exception("Database error while fetching job applications", sqlEx);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                throw new Exception("An unexpected error occurred while fetching job applications", ex);
            }

            return applications;
        }



       
        public JobApplication GetApplicationById(int applicationId)
        {
            JobApplication application = null;

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_jobportal_getapplicationbyid", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@p_ApplicationId", applicationId);

                        // Output parameters
                        var p_JobId = new SqlParameter("@p_JobId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        var p_ResumeUrl = new SqlParameter("@p_ResumeUrl", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                        var p_CoverLetter = new SqlParameter("@p_CoverLetter", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Output };
                        var p_AppliedDate = new SqlParameter("@p_AppliedDate", SqlDbType.DateTime) { Direction = ParameterDirection.Output };
                        var p_CurrentStatus = new SqlParameter("@p_CurrentStatus", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                        var p_Education = new SqlParameter("@p_Education", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                        var p_Experience = new SqlParameter("@p_Experience", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                        var p_Skills = new SqlParameter("@p_Skills", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                        var p_ProfileSummary = new SqlParameter("@p_ProfileSummary", SqlDbType.NVarChar, 1000) { Direction = ParameterDirection.Output };
                        var p_ProfilePictureUrl = new SqlParameter("@p_ProfilePictureUrl", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                        var p_Mobile = new SqlParameter("@p_Mobile", SqlDbType.NVarChar, 20) { Direction = ParameterDirection.Output };
                        var p_Gender = new SqlParameter("@p_Gender", SqlDbType.NVarChar, 10) { Direction = ParameterDirection.Output };
                        var p_DateOfBirth = new SqlParameter("@p_DateOfBirth", SqlDbType.DateTime) { Direction = ParameterDirection.Output };
                        var p_ErrorStatusCode = new SqlParameter("@p_ErrorStatusCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        var p_ErrorMessage = new SqlParameter("@p_ErrorMessage", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };

                        // Add output parameters to command
                        cmd.Parameters.AddRange(new[]
                        {
                    p_JobId, p_ResumeUrl, p_CoverLetter, p_AppliedDate, p_CurrentStatus,
                    p_Education, p_Experience, p_Skills, p_ProfileSummary, p_ProfilePictureUrl,
                    p_Mobile, p_Gender, p_DateOfBirth, p_ErrorStatusCode, p_ErrorMessage
                });

                        con.Open();
                        cmd.ExecuteNonQuery();

                        int statusCode = (int)p_ErrorStatusCode.Value;
                        string message = p_ErrorMessage.Value?.ToString();

                        if (statusCode == 1)
                        {
                            application = new JobApplication
                            {
                                ApplicationId = applicationId,
                                JobId = (int)p_JobId.Value,
                                ResumeUrl = p_ResumeUrl.Value?.ToString(),
                                CoverLetter = p_CoverLetter.Value?.ToString(),

                               
                                

                                AppliedDate = p_AppliedDate.Value is DateTime dt ? dt : throw new InvalidOperationException("AppliedDate cannot be null"),
                                CurrentStatus = p_CurrentStatus.Value?.ToString(),
                                Education = p_Education.Value?.ToString(),
                                Experience = p_Experience.Value?.ToString(),
                                Skills = p_Skills.Value?.ToString(),
                                ProfileSummary = p_ProfileSummary.Value?.ToString(),
                                ProfilePictureUrl = p_ProfilePictureUrl.Value?.ToString(),
                                Mobile = p_Mobile.Value?.ToString(),
                                Gender = p_Gender.Value?.ToString(),
                                DateOfBirth = p_DateOfBirth.Value != DBNull.Value && p_DateOfBirth.Value is DateTime dateOfBirth ? dateOfBirth : default
                               
                            };
                        }
                        else
                        {
                            
                            throw new ApplicationException("Stored Procedure Error: " + message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("An error occurred while fetching job application by ID.", ex);
            }

            return application;
        }

        
        public bool AddStatus(ApplicationStatus status)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_jobportal_addstatus", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Input parameters
                        cmd.Parameters.AddWithValue("@p_ApplicationId", status.ApplicationId);
                        cmd.Parameters.AddWithValue("@p_Status", status.Status);
                        cmd.Parameters.AddWithValue("@p_StatusUpdatedAt", status.StatusUpdatedAt);
                        cmd.Parameters.AddWithValue("@p_Remarks", (object?)status.Remarks ?? DBNull.Value);

                        // Output parameters
                        var p_ErrorStatusCode = new SqlParameter("@p_ErrorStatusCode", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        var p_ErrorMessage = new SqlParameter("@p_ErrorMessage", SqlDbType.NVarChar, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };

                        cmd.Parameters.Add(p_ErrorStatusCode);
                        cmd.Parameters.Add(p_ErrorMessage);

                        con.Open();
                        cmd.ExecuteNonQuery();

                        int statusCode = (int)p_ErrorStatusCode.Value;
                        string message = p_ErrorMessage.Value?.ToString();

                        if (statusCode == 1)
                        {
                            return true;
                        }
                        else
                        {
                            
                            throw new ApplicationException("Stored Procedure Error: " + message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("An error occurred while adding application status.", ex);
            }
        }


       
        public bool UpdateStatus(ApplicationStatus status)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_jobportal_updatestatus", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Input parameters
                        cmd.Parameters.AddWithValue("@p_ApplicationId", status.ApplicationId);
                        cmd.Parameters.AddWithValue("@p_Status", status.Status);
                        cmd.Parameters.AddWithValue("@p_StatusUpdatedAt", status.StatusUpdatedAt);
                        cmd.Parameters.AddWithValue("@p_Remarks", (object?)status.Remarks ?? DBNull.Value);

                        // Output parameters
                        SqlParameter p_ErrorStatusCode = new SqlParameter("@p_ErrorStatusCode", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        SqlParameter p_ErrorMessage = new SqlParameter("@p_ErrorMessage", SqlDbType.NVarChar, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };

                        cmd.Parameters.Add(p_ErrorStatusCode);
                        cmd.Parameters.Add(p_ErrorMessage);

                        con.Open();
                        cmd.ExecuteNonQuery();

                        int statusCode = (int)p_ErrorStatusCode.Value;
                        string message = p_ErrorMessage.Value?.ToString();

                        if (statusCode == 1)
                        {
                            return true;
                        }
                        else
                        {
                            throw new ApplicationException("Stored Procedure Error: " + message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("An error occurred while updating application status.", ex);
            }
        }

    }
}
