using Microsoft.Owin.Security.DataHandler;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class CertificateCreationViewModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select an Item From Menu above")]
        public int CandidateId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select an Item From Menu above")]
        public int MicroCredentialId { get; set; }
        [Required]
        [Range(1, int.MaxValue,ErrorMessage = "Please Select an Item From Menu above")]
        public int MicroCredentialBadgeId { get; set; }
        [Required]
        public string CandidateFullName { get; set; }
        [Required]
        public string CertificateTextContent { get; set; }
    }
}