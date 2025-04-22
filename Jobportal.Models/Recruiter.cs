using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobportal.Models
{
    public class Recruiter
    {
        public int RecruiterId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class RecruiterRegisterRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string CompanyName { get; set; }
    }

    public class RecruiterLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RecruiterLoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Recruiter? Data { get; set; }
    }
}
