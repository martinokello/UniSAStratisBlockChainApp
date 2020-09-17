using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class MicroCredentialBadgeWithMicroCredentialNameViewModel
    {
        [Required]
        public int MicroCredentialBadgeId { get; set; }
        public int CandidateId { get; set; }
        public int MicroCredentialId { get; set; }
        public string MicroCredentialName { get; set; }
        public string Username { get; set; }
        public string MicroCredentialBadges { get; set; }
        public string Select { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
    }
}