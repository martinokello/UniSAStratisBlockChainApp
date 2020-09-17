using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UniSA.Domain;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class CandidateMicroCredentialCourseViewModel
    {
        public int CandidateMicroCredentialCourseId { get; set; }
        [Required]
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        [Required]
        public int MicroCredentialId { get; set; }
        public MicroCredential MicroCredential { get; set; }

        public int MicroCredentialBadgeId { get; set; }
        public UserMicroCredentialBadges MicroCredentialBadge { get; set; }

        public bool HasPassed { get; set; }
        public string HashOfMine { get; set; }
        public string Select { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
    }
}