using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniSA.Domain
{
    public class Government
    {
        [Key]
        public int GovernmentDepartmentId { get; set; }
        public string Country { get; set; }
        public string GovernmentDepartmentName { get; set; }
        [ForeignKey("DepartmentAddress")]
        public int DepartmentAddressId { get; set; }
        public Address DepartmentAddress { get; set; }
        public string GovernmentDepartmentContactNumber { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber{ get; set; }
        public string ContactEmailAddress { get; set; }
    }
}
