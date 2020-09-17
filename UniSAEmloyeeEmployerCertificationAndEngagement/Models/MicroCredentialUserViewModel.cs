using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class UserMicroCredentialBadgesViewModel
    {
        public int MicroCredentialBadgeId { get; set; }
        [Required]
        public int CandidateId { get; set; }
        [Required]
        public int MicroCredentialId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string MicroCredentialBadges { get; set; }
        public string Select { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
    }
}