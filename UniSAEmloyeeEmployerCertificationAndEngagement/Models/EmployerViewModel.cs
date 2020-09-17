using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class EmployerViewModel
    {
        public int EmployerId { get; set; }
        [Required]
        public string EmployerName { get; set; }
        [Required]
        public int AddressId { get; set; }
        [Required]
        public string ContactPerson { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        [Required]
        public string ContactEmailAddress { get; set; }
        [Required]
        public string Sector { get; set; }
        public string Select { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
    }
}