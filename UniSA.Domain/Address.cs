using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace UniSA.Domain
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2{ get; set; }
        public string Town { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
    }
}
