using System;
using System.Linq;
using System.Collections.Generic;
using UniSA.Domain;
using UniSA.DataAccess.Abstracts;

namespace UniSA.DataAccess.Concretes
{
    public class MicroCredentialRepository : AbstractRepository<MicroCredential>
    {
        public UniSADbContext UniSADbContextInstance { get; set; }

        public override MicroCredential GetById(int id)
        {
            return UniSADbContextInstance.MicroCredentials.FirstOrDefault(p => p.MicroCredentialId == id);
        }

        public override bool Update(MicroCredential item)
        {
            try
            {
                var toUpdate = UniSADbContextInstance.MicroCredentials.FirstOrDefault(p => p.MicroCredentialId == item.MicroCredentialId);

                toUpdate.MicroCredentialCode = item.MicroCredentialCode;
                toUpdate.MicroCredentialDescription = item.MicroCredentialDescription;
                toUpdate.MicroCredentialName = item.MicroCredentialName;
                toUpdate.NumberOfCredits = item.NumberOfCredits;
                toUpdate.DurationStart = item.DurationStart;
                toUpdate.NumberOfCredits = item.NumberOfCredits;
                toUpdate.DurationEnd = item.DurationEnd;
                toUpdate.CertificateFee = item.CertificateFee;
                toUpdate.DigitalBadge = item.DigitalBadge;
                toUpdate.AccreditationBodyId = item.AccreditationBodyId;
                toUpdate.EndorsementBodyId = item.EndorsementBodyId;
                toUpdate.IsAccredited = item.IsAccredited;
                toUpdate.IsEndorsed = item.IsEndorsed;
                toUpdate.Fee = item.Fee;
                toUpdate.MoocProviderId = item.MoocProviderId;
                //toUpdate.JobId = item.JobId;
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CreateMicroCredential(MicroCredential microCredential)
        {
            throw new NotImplementedException();
        }
    }
}
