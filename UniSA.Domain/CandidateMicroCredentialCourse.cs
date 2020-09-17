using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UniSA.Domain
{
    public class CandidateMicroCredentialCourse
    {
        [Key]
        public int CandidateMicroCredentialCourseId { get; set; }
        [ForeignKey("Candidate")]
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        [ForeignKey("MicroCredential")]
        public int MicroCredentialId { get; set; }
        public MicroCredential MicroCredential { get; set; }
        public int MicroCredentialBadgeId { get; set; }
        public bool HasPassed { get; set; }
        public string HashOfMine{get;set;}
        public string MineBlockContent { get; set; }
        public string MineAddressTip { get; set; }
        public string transactionHex { get; set; }
        public string transactionId { get; set; }
    }
}
