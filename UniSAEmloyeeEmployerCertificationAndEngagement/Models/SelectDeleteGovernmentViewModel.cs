using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class SelectDeleteGovernmentViewModel
    {
        [Required]
        public int GovernmentDepartmentId { get; set; }
        public string Country { get; set; }
        public string GovernmentDepartmentName { get; set; }
        public int DepartmentAddressId { get; set; }
        public string GovernmentDepartmentContactNumber { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmailAddress { get; set; }
        public string Select { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
    }
}