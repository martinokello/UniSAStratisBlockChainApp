using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class MoocProviderViewModel 
    {
        public int MoocProviderId { get; set; }
        [Required]
        public string MoocProviderNumber { get; set; }
        [Required]
        public string MoocProviderContactNumber { get; set; }
        [Required]
        public string MoocProviderName { get; set; }
        public int AddressId { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        public string Select { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
    }
}