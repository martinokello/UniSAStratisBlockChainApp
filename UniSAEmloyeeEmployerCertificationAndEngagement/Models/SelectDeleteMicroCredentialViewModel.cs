using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class SelectDeleteMicroCredentialViewModel
    {
        [Required]
        public int MicroCredentialId { get; set; }
        public int MoocProviderId { get; set; }
        public string MicroCredentialCode { get; set; }
        public string MicroCredentialName { get; set; }
        public string MicroCredentialDescription { get; set; }
        public int NumberOfCredits { get; set; }
        public int JobId { get; set; }
        public int AccreditedBodyId { get; set; }
        public decimal Fee { get; set; }
        public decimal CertificateFee { get; set; }
        public DateTime DurationStart { get; set; }
        public DateTime DurationEnd { get; set; } 
        public bool IsAccredited { get; set; }
        public bool IsEndorsed { get; set; }
        public int EndorsedBodyId { get; set; }
        public string Select { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
    }
}