using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jobportal.Models;
using Jobportal.Provider;
namespace Jobportal.Service
{
    public interface IRecruiterService
    {
        bool CreateRecruiterProfile(RecruiterProfile profile);
        (RecruiterProfile Profile, string Message) GetRecruiterProfile(int recruiterId);
        bool UpdateRecruiterProfile(RecruiterProfile profile);
        object AddJob(Job job);
        bool DeleteJob(int jobId);
        List<JobApplication> GetApplicationsByJobSeeker(int jobSeekerId);
        JobApplication GetApplicationById(int applicationId);
        bool AddStatus(ApplicationStatus status);
        bool UpdateStatus(ApplicationStatus status);
       List<Job> GetJobsByRecruiterId(int recruiterId);
    }

    public class RecruiterService : IRecruiterService
    {
        private readonly RecruiterProvider _recruiterProvider;

        public RecruiterService(RecruiterProvider recruiterProvider)
        {
            _recruiterProvider = recruiterProvider;
        }

        public bool CreateRecruiterProfile(RecruiterProfile profile)
        {
            var result = _recruiterProvider.CreateRecruiterProfile(profile);
            return result.Success; 
        }

       
        public (RecruiterProfile Profile, string Message) GetRecruiterProfile(int recruiterId)
        {
            
            return _recruiterProvider.GetRecruiterProfile(recruiterId);
        }
        public bool UpdateRecruiterProfile(RecruiterProfile profile)
        {
            return _recruiterProvider.UpdateRecruiterProfile(profile);
        }

        public object AddJob(Job job)
        {
            return _recruiterProvider.AddJob(job);
        }

        public bool DeleteJob(int jobId)
        {
            return _recruiterProvider.DeleteJob(jobId);
        }
        public List<Job> GetJobsByRecruiterId(int recruiterId)
        {
            return _recruiterProvider.GetJobsByRecruiterId(recruiterId);
        }
        public List<JobApplication> GetApplicationsByJobSeeker(int jobSeekerId)
        {
            return _recruiterProvider.GetApplicationsByJobSeeker(jobSeekerId);
        }

        public JobApplication GetApplicationById(int applicationId)
        {
            return _recruiterProvider.GetApplicationById(applicationId);
        }

        public bool AddStatus(ApplicationStatus status)
        {
            return _recruiterProvider.AddStatus(status);
        }

        public bool UpdateStatus(ApplicationStatus status)
        {
            return _recruiterProvider.UpdateStatus(status);
        }
    }
}


