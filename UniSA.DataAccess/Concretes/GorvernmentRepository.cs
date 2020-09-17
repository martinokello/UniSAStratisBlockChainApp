using System;
using System.Linq;
using System.Collections.Generic;
using UniSA.Domain;
using UniSA.DataAccess.Abstracts;

namespace UniSA.DataAccess.Concretes
{
    public class GorvernmentRepository : AbstractRepository<Government>
    {
        public UniSADbContext UniSADbContextInstance { get; set; }

        public override Government GetById(int id)
        {
            return UniSADbContextInstance.Governments.FirstOrDefault(p => p.DepartmentAddressId == id);
        }

        public override bool Update(Government item)
        {
            try
            {
                var toUpdate = UniSADbContextInstance.Governments.FirstOrDefault(p => p.DepartmentAddressId == item.DepartmentAddressId);

                toUpdate.DepartmentAddressId = item.DepartmentAddressId;
                toUpdate.ContactEmailAddress = item.ContactEmailAddress;
                toUpdate.ContactNumber = item.ContactNumber;
                toUpdate.ContactName = item.ContactName;
                toUpdate.Country = item.Country;
                toUpdate.DepartmentAddressId = item.DepartmentAddressId;
                toUpdate.GovernmentDepartmentName = item.GovernmentDepartmentName;
                toUpdate.GovernmentDepartmentContactNumber = item.GovernmentDepartmentContactNumber;
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
