using System;
using System.Linq;
using System.Collections.Generic;
using UniSA.Domain;
using UniSA.DataAccess.Abstracts;

namespace UniSA.DataAccess.Concretes
{
    public class EmployerRepository : AbstractRepository<Employer>
    {
        public UniSADbContext UniSADbContextInstance { get; set; }

        public override Employer GetById(int id)
        {
            return UniSADbContextInstance.Employers.FirstOrDefault(p => p.EmployerId == id);
        }

        public override bool Update(Employer item)
        {
            try
            {
                var toUpdate = UniSADbContextInstance.Employers.FirstOrDefault(p => p.EmployerId == item.EmployerId);

                toUpdate.AddressId = item.AddressId;
                toUpdate.ContactEmailAddress = item.ContactEmailAddress;
                toUpdate.ContactNumber = item.ContactNumber;
                toUpdate.ContactPerson = item.ContactPerson;
                toUpdate.EmployerName = item.EmployerName;
                toUpdate.Sector = item.Sector;
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
