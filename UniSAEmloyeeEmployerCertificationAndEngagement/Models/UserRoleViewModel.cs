using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        [Required(ErrorMessage ="Username Required!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Role Name Required!")]
        public string RoleName { get; set; }
        public string Insert { get; set; }
        public string Delete { get; set; }
    }
}