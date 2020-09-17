using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class GovernmentViewModel
    {
        public int GovernmentDepartmentId { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string GovernmentDepartmentName { get; set; }
        [Required]
        public int DepartmentAddressId { get; set; }
        [Required]
        public string GovernmentDepartmentContactNumber { get; set; }
        [Required]
        public string ContactName { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        [Required]
        public string ContactEmailAddress { get; set; }
        public string Select { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
    }
}