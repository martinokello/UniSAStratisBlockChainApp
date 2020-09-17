using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniSA.DataAccess.Abstracts;
using UniSA.Domain;

namespace UniSA.DataAccess.Concretes
{
    public class EndorsementBodyRepository : AbstractRepository<EndorsementBody>
    {
         public UniSADbContext UniSADbContextInstance { get; set; }

        public override EndorsementBody GetById(int id)
        {
            return UniSADbContextInstance.EndorsementBodies.FirstOrDefault(p => p.EndorsementBodyId == id);
        }

        public override bool Update(EndorsementBody item)
        {
            try
            {
                var toUpdate = UniSADbContextInstance.EndorsementBodies.FirstOrDefault(p => p.EndorsementBodyId == item.EndorsementBodyId);

                toUpdate.EndorsementBodyName = item.EndorsementBodyName;
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
