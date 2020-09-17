using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class SelectDeleteJobViewModel
    {
        [Required]
        public int JobId { get; set; }
        public bool QualificationsRequired { get; set; }
        public int MicroCredentialId { get; set; }
        public string JobCode { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public int NumberOfPositions { get; set; }
        public int EmployerId { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime DateUpdated { get; set; } 
        public string Select { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
    }
}