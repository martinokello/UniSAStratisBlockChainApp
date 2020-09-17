using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class ValidateUserMicroCredentialBadges
    {
        [Required]
        public int MicroCredentialBadgeId { get; set; }
        [Required]
        public int CandidateId { get; set; }
        [Required]
        public int MicroCredentialId { get; set; }
        [Required]
        public string Username { get; set; }
    }
}