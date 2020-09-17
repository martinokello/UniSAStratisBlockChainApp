using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Models
{
    public class EmployerJobsViewModel
    {
        public EmployerViewModel Employer { get; set; }
        public JobViewModel[] Jobs { get; set; }
    }
}