using System;
using System.Linq;
using System.Collections.Generic;
using UniSA.Domain;
using UniSA.DataAccess.Abstracts;

namespace UniSA.DataAccess.Concretes
{
    public class MoocProviderRepository : AbstractRepository<MoocProvider>
    {
        public UniSADbContext UniSADbContextInstance { get; set; }

        public override MoocProvider GetById(int id)
        {
            return UniSADbContextInstance.MoocProviders.FirstOrDefault(p => p.MoocProviderId == id);
        }

        public override bool Update(MoocProvider item)
        {
            try
            {
                var toUpdate = UniSADbContextInstance.MoocProviders.FirstOrDefault(p => p.MoocProviderId == item.MoocProviderId);

                toUpdate.MoocProviderName = item.MoocProviderName;
                toUpdate.MoocProviderNumber = item.MoocProviderNumber;
                toUpdate.AddressId = item.AddressId;
                toUpdate.EmailAddress = item.EmailAddress;
                toUpdate.MoocProviderContactNumber = item.MoocProviderContactNumber;
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
