using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobportal.Models
{
    public class RecruiterProfile
    {
        public int ProfileId { get; set; }         // Auto-incremented primary key in DB
        public int RecruiterId { get; set; }       // Foreign key to Recruiter table
        public string CompanyName { get; set; }
        public string RecruiterName { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Designation { get; set; }
        public string Location { get; set; }
        public string AboutCompany { get; set; }
    }
}
