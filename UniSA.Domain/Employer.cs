using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniSA.Domain
{
    public class Employer
    {
        [Key]
        public int EmployerId { get; set; }
        public string EmployerName { get; set; }
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address Address { get;set;}
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmailAddress { get; set; }
        public string Sector { get; set; }

    }
}
