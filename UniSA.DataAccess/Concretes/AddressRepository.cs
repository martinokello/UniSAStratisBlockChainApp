using System;
using System.Linq;
using System.Collections.Generic;
using UniSA.Domain;
using UniSA.DataAccess.Abstracts;

namespace UniSA.DataAccess.Concretes
{
    public class AddressRepository : AbstractRepository<Address>
    {
        public UniSADbContext UniSADbContextInstance { get; set; }
        public override UniSA.Domain.Address GetById(int id)
        {
            return UniSADbContextInstance.Addresses.FirstOrDefault(p => p.AddressId == id);
        }

        public override bool Update(Address item)
        {
            try
            {
                var toUpdate = UniSADbContextInstance.Addresses.FirstOrDefault(p => p.AddressId == item.AddressId);

                toUpdate.AddressLine1 = item.AddressLine1;
                toUpdate.AddressLine2 = item.AddressLine2;
                toUpdate.Country = item.Country;
                toUpdate.PostCode = item.PostCode;
                toUpdate.Town = item.Town;
                return true;
            }
            catch(Exception e)
            {
                throw e;
            }

        }
    }
}
