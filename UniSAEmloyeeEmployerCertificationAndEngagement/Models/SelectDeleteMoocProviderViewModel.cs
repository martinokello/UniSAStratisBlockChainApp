using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class SelectDeleteMoocProviderViewModel
    {
        [Required]
        public int MoocProviderId { get; set; }
        public string MoocProviderNumber { get; set; }
        public string MoocProviderContactNumber { get; set; }
        public string MoocProviderName { get; set; }
        public int MicroCredentialId { get; set; }
        public List<MicroCredentialViewModel> MicroCredentials { get; set; }
        public int AddressId { get; set; }
        public string EmailAddress { get; set; }
        public string Select { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
    }
}