using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniSA.Domain
{
    public class RecruitmentAgency
    {
        [Key]
        public int RecruitmentAgencyId { get; set; }
        public string RecruitmentAgencyName { get; set; }
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmailAddress { get; set; }
        [ForeignKey("JobAdvertised")]
        public int JobAdvertisedId { get; set; }
        public Job JobAdvertised { get; set; }
    }
}
