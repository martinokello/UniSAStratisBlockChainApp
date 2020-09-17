using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSA.DataAccess.Abstracts;
using UniSA.Domain;

namespace UniSA.DataAccess.Concretes
{
    public class CandidateJobApplicationRepository : AbstractRepository<CandidateJobApplication>
    {
        public UniSADbContext UniSADbContextInstance { get; set; }

        public override CandidateJobApplication GetById(int id)
        {
            return UniSADbContextInstance.CandidateJobApplications.FirstOrDefault(cj => cj.CandidateJobApplicationId == id);
        }

        public override bool Update(CandidateJobApplication item)
        {
            try
            {
                var cjApplication = UniSADbContextInstance.CandidateJobApplications.FirstOrDefault(cj => cj.CandidateJobApplicationId == item.CandidateJobApplicationId);
                cjApplication.JobId = item.JobId;
                cjApplication.EmployerId = item.EmployerId;
                cjApplication.CandidateId = item.CandidateId;
                cjApplication.IsCertified = item.IsCertified;
                cjApplication.IsFullyPaidForCourse = item.IsFullyPaidForCourse;
                return true;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
