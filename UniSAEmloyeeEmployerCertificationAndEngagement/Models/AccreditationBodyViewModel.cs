using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class AccreditationBodyViewModel
    {
        public int AccreditationBodyId { get; set; }
        [Required]
        public string AccreditationBodyName { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        [Required]
        public int AddressId { get; set; }

        public string Select { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
    }
}