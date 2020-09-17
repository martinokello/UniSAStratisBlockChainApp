using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UniSA.Domain
{
    public class EndorsementBody
    {
        [Key]
        public int EndorsementBodyId { get; set; }
        public string EndorsementBodyName { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address Address { get; set; }
    }
}
