using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobportal.Models
{
    // Models/JobSeeker.cs
    public class JobSeeker
    {
        public int JobSeekerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    // Models/RegisterRequest.cs
    public class RegisterRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    // Models/LoginRequest.cs
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    // Models/LoginResponse.cs
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int? JobSeekerId { get; set; }
    }


}
