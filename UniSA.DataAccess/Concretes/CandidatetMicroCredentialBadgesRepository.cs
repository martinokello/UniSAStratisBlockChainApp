using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSA.DataAccess.Abstracts;
using UniSA.Domain;

namespace UniSA.DataAccess.Concretes
{
    public class CandidatetMicroCredentialBadgesRepository : AbstractRepository<UserMicroCredentialBadges>
    {
        public UniSADbContext UniSADbContextInstance { get; set; }
        public override UserMicroCredentialBadges GetById(int id)
        {
            return UniSADbContextInstance.UserMicroCredentialBadgess.FirstOrDefault(ub => ub.MicroCredentialBadgeId == id);
        }

        public override bool Update(UserMicroCredentialBadges item)
        {
            try
            {
                var usBadge = UniSADbContextInstance.UserMicroCredentialBadgess.FirstOrDefault(ub => ub.MicroCredentialBadgeId == item.MicroCredentialBadgeId);
                usBadge.MicroCredentialBadges = item.MicroCredentialBadges;
                usBadge.Username = item.Username;
                usBadge.MicroCredentialId = item.MicroCredentialId;
                usBadge.CandidateId = item.CandidateId;
                return true;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
