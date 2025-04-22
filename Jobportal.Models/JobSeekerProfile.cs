using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobportal.Models
{
    public class JobSeekerProfile
    {
        public int ProfileId { get; set; }
        public int JobSeekerId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string Skills { get; set; }
        public string ProfilePhoto { get; set; }
        public string CV { get; set; }
    }
}
