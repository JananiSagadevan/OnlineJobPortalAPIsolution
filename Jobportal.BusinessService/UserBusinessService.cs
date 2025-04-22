using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jobportal.Models;
using Jobportal.Provider;
using Jobportal.Service;
namespace Jobportal.BusinessService
{
    public interface IUserBusinessService
    {
        object CreateProfile(JobSeekerProfile profile);
        (JobSeekerProfile Profile, int StatusCode, string Message) JobSeekerProfiles(int jobSeekerId);
        (bool IsSuccess, int StatusCode, string Message) UpdateProfile(int jobSeekerId, JobSeekerProfile profile);
        (List<Job> Jobs, int StatusCode, string Message) GetAllJobs();
        (Job Job, int StatusCode, string Message) GetJobById(int jobId);
        (bool IsSuccess, int StatusCode, string Message) SubmitApplication(JobApplication app);
        (List<ApplicationStatus> History, bool IsSuccess, int StatusCode, string Message) GetStatusHistory(int applicationId);
    }
    public class UserBusinessService : IUserBusinessService
    {
        private readonly IUserService _userService;

        public UserBusinessService(IUserService userService)
        {
            _userService = userService;
        }

        public object CreateProfile(JobSeekerProfile profile)
        {
            try
            {
                return _userService.CreateProfile(profile);
            }
            catch (Exception ex)
            {
                return new
                {
                    StatusCode = -99,
                    Message = $"Business Error: {ex.Message}"
                };
            }
        }

        public (JobSeekerProfile Profile, int StatusCode, string Message) JobSeekerProfiles(int jobSeekerId)
        {
            try
            {
                return _userService.JobSeekerProfiles(jobSeekerId);
            }
            catch (Exception ex)
            {
                return (null, -99, $"Business Error: {ex.Message}");
            }
        }

        public (bool IsSuccess, int StatusCode, string Message) UpdateProfile(int jobSeekerId, JobSeekerProfile profile)
        {
            try
            {
                return _userService.UpdateProfile(jobSeekerId, profile);
            }
            catch (Exception ex)
            {
                return (false, -99, $"Business Error: {ex.Message}");
            }
        }

        public (List<Job> Jobs, int StatusCode, string Message) GetAllJobs()
        {
            try
            {
                return _userService.GetAllJobs();
            }
            catch (Exception ex)
            {
                return (new List<Job>(), -99, $"Business Error: {ex.Message}");
            }
        }

        public (Job Job, int StatusCode, string Message) GetJobById(int jobId)
        {
            try
            {
                return _userService.GetJobById(jobId);
            }
            catch (Exception ex)
            {
                return (null, -99, $"Business Error: {ex.Message}");
            }
        }

        public (bool IsSuccess, int StatusCode, string Message) SubmitApplication(JobApplication app)
        {
            try
            {
                return _userService.SubmitApplication(app);
            }
            catch (Exception ex)
            {
                return (false, -99, $"Business Error: {ex.Message}");
            }
        }

        public (List<ApplicationStatus> History, bool IsSuccess, int StatusCode, string Message) GetStatusHistory(int applicationId)
        {
            try
            {
                return _userService.GetStatusHistory(applicationId);
            }
            catch (Exception ex)
            {
                return (new List<ApplicationStatus>(), false, -99, $"Business Error: {ex.Message}");
            }
        }
    }
}
