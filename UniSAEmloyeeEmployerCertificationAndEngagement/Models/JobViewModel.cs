using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class JobViewModel
    {
        public int JobId { get; set; }
        public bool QualificationsRequired { get; set; }
        public int MicroCredentialId { get; set; }
        [Required]
        public string JobCode { get; set; }
        [Required]
        public string JobTitle { get; set; }
        [Required]
        public string JobDescription { get; set; }
        [Required]
        public int NumberOfPositions { get; set; }
        [Required]
        public int EmployerId { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime DateUpdated { get; set; }
        public bool IsCertified { get; set; }
        public bool IsFullyPaidForCourse { get; set; }
        public HttpPostedFileBase CVFile { get; set; }
        public HttpPostedFileBase CoverLetter { get; set; }
        public string Select { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }

        public override bool Equals(object obj)
        {
            return (obj as JobViewModel).JobId == this.JobId && (obj as JobViewModel).EmployerId == this.EmployerId;
        }

        public override int GetHashCode()
        {
            return this.EmployerId + this.JobId;
        }
    }
}