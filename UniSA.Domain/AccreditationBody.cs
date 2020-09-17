using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniSA.Domain
{
    public class AccreditationBody
    {
        [Key]
        public int AccreditationBodyId { get; set; }
        public string AccreditationBodyName { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address Address { get; set; }
    }
}
