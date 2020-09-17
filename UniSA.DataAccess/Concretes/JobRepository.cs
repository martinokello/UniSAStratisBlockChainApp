using System;
using System.Linq;
using System.Collections.Generic;
using UniSA.Domain;
using UniSA.DataAccess.Abstracts;

namespace UniSA.DataAccess.Concretes
{
    public class JobRepository : AbstractRepository<Job>
    {
        public UniSADbContext UniSADbContextInstance { get; set; }

        public override Job GetById(int id)
        {
            return UniSADbContextInstance.Jobs.FirstOrDefault(p => p.JobId == id);
        }

        public override bool Update(Job item)
        {
            try
            {
                var toUpdate = UniSADbContextInstance.Jobs.FirstOrDefault(p => p.JobId == item.JobId);

                toUpdate.JobTitle = item.JobTitle;
                toUpdate.JobCode = item.JobCode;
                toUpdate.NumberOfPositions = item.NumberOfPositions;
                toUpdate.JobDescription = item.JobDescription;
                toUpdate.IsActive = item.IsActive;
                toUpdate.MicroCredentialId = item.MicroCredentialId;
                toUpdate.MicroCredentialRequired = item.MicroCredentialRequired;
                toUpdate.QualificationsRequired = item.QualificationsRequired;
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
