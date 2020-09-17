using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UniSA.Domain
{
    public class UserMicroCredentialBadges
    {
        [Key]
        public int MicroCredentialBadgeId { get; set; }
        [ForeignKey("Candidate")]
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        [ForeignKey("MicroCredential")]
        public int MicroCredentialId { get; set; }
        public MicroCredential MicroCredential { get; set; }
        public string Username { get; set; }
        public string MicroCredentialBadges { get; set; }
    }
}
