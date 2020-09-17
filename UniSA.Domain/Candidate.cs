using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniSA.Domain
{
    public class Candidate
    {
        [Key]
        public int CandidateId { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public List<MicroCredential> UserMicroCredentials { get; set; }
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public string ContactNumber { get; set; }
        public string HighestQualification { get; set; }
        [ForeignKey("AppliedJob")]
        public int AppliedJobsId { get; set; }
        public Job AppliedJob { get; set; }
        public string DigitalBadgeEarned { get; set; }
        [ForeignKey("MicroCredentialEnrolled")]
        public int MicroCredentialEnrolledId { get; set; }
        public MicroCredential MicroCredentialEnrolled { get; set; }
    }
}
