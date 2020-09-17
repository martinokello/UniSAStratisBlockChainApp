﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class SelectDeleteAccreditationBodyViewModel 
    {
        [Required]
        public int AccreditationBodyId { get; set; }
        public string AccreditationBodyName { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public int AddressId { get; set; }
        public string Select { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
    }
}