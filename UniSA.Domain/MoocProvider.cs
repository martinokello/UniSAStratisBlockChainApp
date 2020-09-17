using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniSA.Domain
{
    public class MoocProvider
    {
        [Key]
        public int MoocProviderId { get; set; }
        public string MoocProviderNumber { get; set; }
        public string MoocProviderContactNumber { get; set; }
        public string MoocProviderName { get; set; }
        [ForeignKey("MoocProviderAddress")]
        public int AddressId { get; set; }
        public Address MoocProviderAddress { get; set; }
        public string EmailAddress { get; set; } = string.Empty;

        public byte[] MoocPublicKey { get; set; }
        public byte[] MoocPrivateKey { get; set; }
        public byte[] MoocModulus { get; set; }
    }
}
