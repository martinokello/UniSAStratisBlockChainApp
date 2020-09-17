using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class MicroCredentialViewModel
    {
        public int MicroCredentialId { get; set; }
        [Required]
        public int MoocProviderId { get; set; }
        [Required]
        public string MicroCredentialCode { get; set; }
        [Required]
        public string MicroCredentialName { get; set; }
        [Required]
        public string MicroCredentialDescription { get; set; }
        [Required]
        public int NumberOfCredits { get; set; }
        public int JobId { get; set; }
        public int AccreditedBodyId { get; set; }
        [Required]
        public decimal Fee { get; set; }
        [Required]
        public decimal CertificateFee { get; set; }
        [Required]
        public DateTime DurationStart { get; set; }
        [Required]
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