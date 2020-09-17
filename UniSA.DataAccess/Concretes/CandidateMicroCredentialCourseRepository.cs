using System;
using System.Linq;
using System.Collections.Generic;
using UniSA.Domain;
using UniSA.DataAccess.Abstracts;
namespace UniSA.DataAccess.Concretes
{
    public class CandidateMicroCredentialCourseRepository : AbstractRepository<CandidateMicroCredentialCourse>
    {
        public UniSADbContext UniSADbContextInstance { get; set; }
        public override UniSA.Domain.CandidateMicroCredentialCourse GetById(int id)
        {
            return UniSADbContextInstance.CandidateMicroCredentialCourses.FirstOrDefault(p => p.CandidateMicroCredentialCourseId == id);
        }

        public override bool Update(CandidateMicroCredentialCourse item)
        {
            try
            {
                var toUpdate = UniSADbContextInstance.CandidateMicroCredentialCourses.FirstOrDefault(p => p.CandidateMicroCredentialCourseId == item.CandidateMicroCredentialCourseId);

                toUpdate.CandidateId = item.CandidateId;
                toUpdate.MicroCredentialId = item.MicroCredentialId;
                toUpdate.transactionId = item.transactionId;
                toUpdate.transactionHex = item.transactionHex;
                toUpdate.HashOfMine = item.HashOfMine;
                toUpdate.MicroCredentialBadgeId = item.MicroCredentialBadgeId;
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
 