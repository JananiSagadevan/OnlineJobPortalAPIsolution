using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jobportal.Models;
using Jobportal.Service;

namespace Jobportal.BusinessService
{
    public interface IAuthBusinessService
    {
        int UserRegister(JobSeeker jobSeeker);
        JobSeeker Login(string email, string password, out int errorCode, out string errorMessage);
        int RecruiterRegister(Recruiter recruiter);
        (Recruiter recruiter, int statusCode, string message) RecruiterLogin(string email, string password);
    }

    public class AuthBusinessService : IAuthBusinessService
    {
        private readonly IAuthService _authService;

        public AuthBusinessService(IAuthService authService)
        {
            _authService = authService;
        }

        public int UserRegister(JobSeeker jobSeeker) => _authService.UserRegister(jobSeeker);
        public JobSeeker Login(string email, string password, out int errorCode, out string errorMessage)
        {
            return _authService.Login(email, password, out errorCode, out errorMessage);
        }
        public int RecruiterRegister(Recruiter recruiter) => _authService.RecruiterRegister(recruiter);
        public (Recruiter recruiter, int statusCode, string message) RecruiterLogin(string email, string password)
        {
            return _authService.RecruiterLogin(email, password);
        }
    
}
}
