using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSA.DataAccess.Interfaces;
using UniSA.Domain;

namespace UniSA.DataAccess.Abstracts
{
    public abstract class AbstractRepository<T> : IRepository<T>  where T : class
    {
        public UniSADbContext DbContextUniSADbContext { get; set; }
        public bool Add(T item)
        {
            try
            {
                DbContextUniSADbContext.Set<T>().Add(item);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool Delete(T item)
        {
            try
            {
                DbContextUniSADbContext.Set<T>().Remove(item);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IQueryable<T> GetAll()
        {
            try
            {
                return DbContextUniSADbContext.Set<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public abstract T GetById(int id);


        public abstract bool Update(T item);
    }
}
