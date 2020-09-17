using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class EnroleForMicroCredentialViewModel
    {
        [Required]
        public int CandidateId { get; set; }
        [Required]
        public int MicroCredentialId { get; set; }
    }
}