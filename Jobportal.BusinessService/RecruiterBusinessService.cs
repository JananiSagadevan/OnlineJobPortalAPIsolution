using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jobportal.Models;
using Jobportal.Service;

namespace Jobportal.BusinessService
{
    public interface IRecruiterBusinessService
    {
        bool CreateRecruiterProfile(RecruiterProfile profile);
        RecruiterProfile GetRecruiterProfile(int recruiterId);
        bool UpdateRecruiterProfile(RecruiterProfile profile);
        object AddJob(Job job);
        bool DeleteJob(int jobId);
        List<JobApplication> GetApplicationsByJobSeeker(int jobSeekerId);
        JobApplication GetApplicationById(int applicationId);
        bool AddStatus(ApplicationStatus status);
        bool UpdateStatus(ApplicationStatus status);
        List<Job> GetJobsByRecruiterId(int recruiterId);
    }
    public class RecruiterBusinessService : IRecruiterBusinessService
    {
        private readonly IRecruiterService _recruiterService;

        public RecruiterBusinessService(IRecruiterService recruiterService)
        {
            _recruiterService = recruiterService;
        }

        public bool CreateRecruiterProfile(RecruiterProfile profile)
        {
            return _recruiterService.CreateRecruiterProfile(profile);
        }

        public RecruiterProfile GetRecruiterProfile(int recruiterId)
        {
            var result = _recruiterService.GetRecruiterProfile(recruiterId);
            return result.Profile; 
        }

        public bool UpdateRecruiterProfile(RecruiterProfile profile)
        {
            return _recruiterService.UpdateRecruiterProfile(profile);
        }

        public object AddJob(Job job)
        {
            return _recruiterService.AddJob(job);
        }

        public bool DeleteJob(int jobId)
        {
            return _recruiterService.DeleteJob(jobId);
        }
        public List<Job> GetJobsByRecruiterId(int recruiterId)
        {
            return _recruiterService.GetJobsByRecruiterId(recruiterId);
        }
        public List<JobApplication> GetApplicationsByJobSeeker(int jobSeekerId) => _recruiterService.GetApplicationsByJobSeeker(jobSeekerId);

        public JobApplication GetApplicationById(int applicationId) => _recruiterService.GetApplicationById(applicationId);

        public bool AddStatus(ApplicationStatus status) => _recruiterService.AddStatus(status);

        public bool UpdateStatus(ApplicationStatus status) => _recruiterService.UpdateStatus(status);
    }
}

