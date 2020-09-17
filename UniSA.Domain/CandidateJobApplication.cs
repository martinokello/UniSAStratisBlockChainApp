using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniSA.Domain
{
    public class CandidateJobApplication
    {
        [Key]
        public int CandidateJobApplicationId { get; set; }
        [ForeignKey("Job")]
        public int JobId { get; set; }
        public Job Job { get; set; }
        [ForeignKey("Candidate")]
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        [ForeignKey("Employer")]
        public int EmployerId { get; set; }
        public Employer Employer { get; set; }
        public bool IsCertified { get; set; }
        public bool IsFullyPaidForCourse { get; set; }
    }
}
