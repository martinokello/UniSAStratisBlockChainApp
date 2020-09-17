using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class SelectDeleteCandidateViewModel
    {
        [Required]
        public int CandidateId { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public List<MicroCredentialViewModel> UserMicroCredentials { get; set; }
        public int AddressId { get; set; }
        public string ContactNumber { get; set; }
        public string HighestQualification { get; set; }
        public int AppliedJobsId { get; set; }
        public string DigitalBadgeEarned { get; set; }
        public int MicroCredentialEnrolledId { get; set; }
        public string Select { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
    }
}