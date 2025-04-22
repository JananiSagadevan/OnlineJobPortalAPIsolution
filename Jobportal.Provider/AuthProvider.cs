using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Jobportal.Models;
using Microsoft.Extensions.Configuration;

namespace Jobportal.Provider
{
    public interface IAuthProvider
    {
        int UserRegister(JobSeeker jobSeeker);
        public JobSeeker LoginUser(string email, string password, out int errorCode, out string errorMessage);
        int RecruiterRegister(Recruiter recruiter);
        (Recruiter recruiter, int statusCode, string message) RecruiterLogin(string email, string password);
    }
    public class AuthProvider : IAuthProvider
    {
        private readonly string _connectionString;

        public AuthProvider(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public int UserRegister(JobSeeker jobSeeker)
        {

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_jobportal_userregister", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            

            cmd.Parameters.AddWithValue("@p_FullName", jobSeeker.FullName);
            cmd.Parameters.AddWithValue("@p_Email", jobSeeker.Email);
            cmd.Parameters.AddWithValue("@p_PasswordHash", jobSeeker.PasswordHash);
            SqlParameter statusCodeParam = new SqlParameter("@p_ErrorStatusCode", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(statusCodeParam);

            SqlParameter messageParam = new SqlParameter("@p_ErrorMessage", SqlDbType.NVarChar, 1000)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(messageParam);
            
            conn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
            cmd.ExecuteNonQuery();

           
        }

       
        public JobSeeker LoginUser(string email, string password, out int errorCode, out string errorMessage)
        {
            JobSeeker jobSeeker = null;
            errorCode = 0;
            errorMessage = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_jobportal_userlogin", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@p_Email", email);
                        cmd.Parameters.AddWithValue("@p_Password", password);

                        SqlParameter statusCodeParam = new SqlParameter("@p_ErrorStatusCode", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(statusCodeParam);

                        SqlParameter messageParam = new SqlParameter("@p_ErrorMessage", SqlDbType.NVarChar, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                jobSeeker = new JobSeeker
                                {
                                    JobSeekerId = Convert.ToInt32(reader["JobSeekerId"]),
                                    FullName = reader["FullName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    PasswordHash = reader["PasswordHash"].ToString(),
                                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                                };
                            }
                        }

                        // Read Output Params
                        errorCode = Convert.ToInt32(statusCodeParam.Value);
                        errorMessage = messageParam.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                errorCode = -99;
                errorMessage = "Exception occurred in provider layer: " + ex.Message;
            }

            return jobSeeker;
        }


        public int RecruiterRegister(Recruiter recruiter)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_jobportal_recruiterregister", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@p_Name", recruiter.Name);
            cmd.Parameters.AddWithValue("@p_Email", recruiter.Email);
            cmd.Parameters.AddWithValue("@p_Password", recruiter.Password);
            cmd.Parameters.AddWithValue("@p_CompanyName", recruiter.CompanyName);

            SqlParameter outIdParam = new SqlParameter("@p_RecruiterId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outIdParam);

            SqlParameter statusCodeParam = new SqlParameter("@p_ErrorStatusCode", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(statusCodeParam);

            SqlParameter messageParam = new SqlParameter("@p_ErrorMessage", SqlDbType.NVarChar, 1000)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(messageParam);

            conn.Open();
            cmd.ExecuteNonQuery();

            return Convert.ToInt32(outIdParam.Value);
        }

   
        public (Recruiter recruiter, int statusCode, string message) RecruiterLogin(string email, string password)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("sp_jobportal_recruiterlogin", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Input parameters
                cmd.Parameters.AddWithValue("@p_Email", email);
                cmd.Parameters.AddWithValue("@p_Password", password);

                // Output parameters
                cmd.Parameters.Add("@p_RecruiterId", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@p_Name", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@p_EmailOut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@p_PasswordOut", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@p_CompanyName", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@p_CreatedAt", SqlDbType.DateTime).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@p_ErrorStatusCode", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@p_ErrorMessage", SqlDbType.NVarChar, 4000).Direction = ParameterDirection.Output;

                conn.Open();
                cmd.ExecuteNonQuery();

                int statusCode = Convert.ToInt32(cmd.Parameters["@p_ErrorStatusCode"].Value);
                string message = cmd.Parameters["@p_ErrorMessage"].Value?.ToString();

                if (statusCode == 1)
                {
                    Recruiter recruiter = new Recruiter
                    {
                        RecruiterId = Convert.ToInt32(cmd.Parameters["@p_RecruiterId"].Value),
                        Name = cmd.Parameters["@p_Name"].Value?.ToString(),
                        Email = cmd.Parameters["@p_EmailOut"].Value?.ToString(),
                        Password = cmd.Parameters["@p_PasswordOut"].Value?.ToString(),
                        CompanyName = cmd.Parameters["@p_CompanyName"].Value?.ToString(),
                        CreatedAt = Convert.ToDateTime(cmd.Parameters["@p_CreatedAt"].Value)
                    };

                    return (recruiter, statusCode, message);
                }

                return (null, statusCode, message);
            }
            catch (Exception ex)
            {
                return (null, -98, "Unhandled error: " + ex.Message);
            }
        }

    }
}
