using System;
using System.Collections.Generic;
using System.Text;
using UniSA.DataAccess;

namespace UniSA.Services.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        UniSADbContext UniSADbContext { get; set; }
        void SaveChanges();

    }
}
