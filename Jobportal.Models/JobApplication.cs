using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobportal.Models
{
    public class JobApplication
    {
        public int ApplicationId { get; set; }
        public int JobSeekerId { get; set; }
        public int JobId { get; set; }
        public string ResumeUrl { get; set; }
        public string CoverLetter { get; set; }
        public DateTime AppliedDate { get; set; }
        public string CurrentStatus { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string Skills { get; set; }
        public string ProfileSummary { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Mobile { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
