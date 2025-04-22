using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobportal.Models
{
    public class Job
    {
        public int JobId { get; set; }
        public int RecruiterId { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public string EmploymentType { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime ApplicationDeadline { get; set; }
    }
}
