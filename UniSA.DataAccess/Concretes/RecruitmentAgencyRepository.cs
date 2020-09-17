using System;
using System.Linq;
using System.Collections.Generic;
using UniSA.Domain;
using UniSA.DataAccess.Abstracts;

namespace UniSA.DataAccess.Concretes
{
    public class RecruitmentAgencyRepository : AbstractRepository<RecruitmentAgency>
    {
        public UniSADbContext UniSADbContextInstance { get; set; }

        public override RecruitmentAgency GetById(int id)
        {
            return UniSADbContextInstance.RecruitmentAgencies.FirstOrDefault(p => p.RecruitmentAgencyId == id);
        }

        public override bool Update(RecruitmentAgency item)
        {
            try
            {
                var toUpdate = UniSADbContextInstance.RecruitmentAgencies.FirstOrDefault(p => p.RecruitmentAgencyId == item.RecruitmentAgencyId);

                toUpdate.RecruitmentAgencyName = item.RecruitmentAgencyName;
                toUpdate.ContactPerson = item.ContactPerson;
                toUpdate.AddressId = item.AddressId;
                toUpdate.ContactEmailAddress = item.ContactEmailAddress;
                toUpdate.JobAdvertisedId = item.JobAdvertisedId;
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
