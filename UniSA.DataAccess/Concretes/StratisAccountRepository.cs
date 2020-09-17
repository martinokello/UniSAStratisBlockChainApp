using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSA.DataAccess.Abstracts;
using UniSA.Domain;

namespace UniSA.DataAccess.Concretes
{
    public class StratisAccountRepository:AbstractRepository<StratisAccount>
    {
        public UniSADbContext UniSADbContextInstance { get; set; }
        public override StratisAccount GetById(int id)
        {
            return UniSADbContextInstance.StratisAccounts.FirstOrDefault(p => p.StratisAccountId == id);
        }

        public override bool Update(StratisAccount item)
        {
            try
            {
                var toUpdate = UniSADbContextInstance.StratisAccounts.FirstOrDefault(p => p.StratisAccountId == item.StratisAccountId);

                toUpdate.AccountName = item.AccountName;
                toUpdate.AccountStratisAddress1 = item.AccountStratisAddress1;
                toUpdate.AccountStratisAddress2 = item.AccountStratisAddress2;
                toUpdate.AccountStratisAddress3 = item.AccountStratisAddress3;
                toUpdate.EmailAddress = item.EmailAddress;
                toUpdate.AccountWalletName = item.AccountWalletName;
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
