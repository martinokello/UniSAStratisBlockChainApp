using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class CandidateJobApplicationViewModel
    {
        public int CandidateJobApplicationId { get; set; }
        [Required]
        public int CandidateId { get; set; }
        [Required]
        public int EmployerId { get; set; }
        [Required]
        public int JobId { get; set; }
        public bool IsCertified { get; set; }
        public bool IsFullyPaidForCourse { get; set; }
        public HttpPostedFileBase CVFile { get; set; }
        public HttpPostedFileBase CoverLetter { get; set; }
    }
}