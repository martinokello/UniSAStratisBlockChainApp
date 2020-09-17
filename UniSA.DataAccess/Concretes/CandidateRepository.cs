using System;
using System.Linq;
using System.Collections.Generic;
using UniSA.Domain;
using UniSA.DataAccess.Abstracts;

namespace UniSA.DataAccess.Concretes
{
    public class CandidateRepository : AbstractRepository<Candidate>
    {
        public UniSADbContext UniSADbContextInstance { get; set; }

        public override Candidate GetById(int id)
        {
            return UniSADbContextInstance.Candidates.FirstOrDefault(p => p.CandidateId == id);
        }

        public override bool Update(Candidate item)
        {
            try
            {
                var toUpdate = UniSADbContextInstance.Candidates.FirstOrDefault(p => p.CandidateId == item.CandidateId);

                toUpdate.AddressId = item.AddressId;
                toUpdate.AppliedJobsId = item.AppliedJobsId;
                toUpdate.HighestQualification = item.HighestQualification;
                toUpdate.MicroCredentialEnrolledId = item.MicroCredentialEnrolledId;
                toUpdate.EmailAddress = item.EmailAddress;
                toUpdate.ContactNumber = item.ContactNumber;
                toUpdate.DigitalBadgeEarned = item.DigitalBadgeEarned;
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
