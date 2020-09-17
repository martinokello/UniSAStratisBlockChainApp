using System;
using System.Linq;
using System.Collections.Generic;
using UniSA.Domain;
using UniSA.DataAccess.Abstracts;


namespace UniSA.DataAccess.Concretes
{
    public class AccreditationBodyRepository : AbstractRepository<AccreditationBody>
    {
        public UniSADbContext UniSADbContextInstance { get; set; }

        public override AccreditationBody GetById(int id)
        {
            return UniSADbContextInstance.AccreditationBodies.FirstOrDefault(p => p.AccreditationBodyId == id);
        }

        public override bool Update(AccreditationBody item)
        {
            try
            {
                var toUpdate = UniSADbContextInstance.AccreditationBodies.FirstOrDefault(p => p.AccreditationBodyId == item.AccreditationBodyId);

                toUpdate.AccreditationBodyName = item.AccreditationBodyName;
                toUpdate.ContactNumber = item.ContactNumber;
                toUpdate.EmailAddress = item.EmailAddress;
                toUpdate.EmailAddress = item.EmailAddress;
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}