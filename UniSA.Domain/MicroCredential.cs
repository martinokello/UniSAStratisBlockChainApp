using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniSA.Domain
{
    public class MicroCredential
    {
        [Key]
        public int MicroCredentialId { get; set; }
        public string MicroCredentialCode { get; set; }
        public string MicroCredentialName { get; set; }
        public string MicroCredentialDescription { get; set; }
        public int NumberOfCredits { get; set; }
        [ForeignKey("AccreditationBody")]
        public int AccreditationBodyId { get; set; }
        public AccreditationBody AccreditationBody { get; set; }
        public decimal Fee { get; set; }
        public decimal CertificateFee { get; set; }
        public DateTime DurationStart { get; set; }
        public DateTime DurationEnd { get; set; }
        public string DigitalBadge { get; set; }
        public bool IsAccredited { get; set; }
        public bool IsEndorsed { get; set; }
        [ForeignKey("EndorsementBody")]
        public int EndorsementBodyId { get; set; }
        public EndorsementBody EndorsementBody { get; set; }
        public int JobId { get; set; }

        [ForeignKey("MoocProvider")]
        public int MoocProviderId { get; set; }
        public MoocProvider MoocProvider { get; set; }
    }
}
