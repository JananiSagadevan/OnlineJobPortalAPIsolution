using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobportal.Models
{
    public class ApplicationStatus
    {
        public int StatusHistoryId { get; set; }
        public int ApplicationId { get; set; }
        public string Status { get; set; }
        public DateTime StatusUpdatedAt { get; set; }
        public string Remarks { get; set; }
    }
}

