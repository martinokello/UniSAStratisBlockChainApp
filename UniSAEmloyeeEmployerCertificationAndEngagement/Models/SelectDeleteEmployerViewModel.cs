using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class SelectDeleteEmployerViewModel
    {
        [Required]
        public int EmployerId { get; set; }
        public string EmployerName { get; set; }
        public int AddressId { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmailAddress { get; set; }
        public string Sector { get; set; }
        public int JobId { get; set; }
        public string MicroCredentialCreatedCode { get; set; }
        public string MicroCredentialEndorsedCode { get; set; }
        public int MicroCredentialId { get; set; }
        public string Select { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
    }
}