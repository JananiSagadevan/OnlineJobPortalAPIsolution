using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jobportal.Models;
using Jobportal.Provider;

namespace Jobportal.Service
{
    public interface IAuthService
    {
        int UserRegister(JobSeeker jobSeeker);
        JobSeeker Login(string email, string password, out int errorCode, out string errorMessage);
        int RecruiterRegister(Recruiter recruiter);
        (Recruiter recruiter, int statusCode, string message) RecruiterLogin(string email, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly IAuthProvider _authProvider;

        public AuthService(IAuthProvider authProvider)
        {
            _authProvider = authProvider;
        }

        public int UserRegister(JobSeeker jobSeeker) => _authProvider.UserRegister(jobSeeker);
        public JobSeeker Login(string email, string password, out int errorCode, out string errorMessage)
        {
            return _authProvider.LoginUser(email, password, out errorCode, out errorMessage);
        }
        public int RecruiterRegister(Recruiter recruiter) => _authProvider.RecruiterRegister(recruiter);
        public (Recruiter recruiter, int statusCode, string message) RecruiterLogin(string email, string password)
        {
            return _authProvider.RecruiterLogin(email, password);
        }
    }
}
