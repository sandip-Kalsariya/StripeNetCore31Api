using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace StripeNetCoreApi.Generic.IGeneric
{
   public interface IGenericRepository<T> where T:class
    {
        List<T> GetAll();
        List<T>  GetwithIncludes(Expression<Func<T, bool>> filter = null,
       Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
       string includeProperties = "");
        T GetbyidwithInclude(Expression<Func<T, bool>> filter = null, string includeProperties = "");

        List<T> GetAllActive();

        IQueryable<T> ListQuery(Expression<Func<T, bool>> where);

        T GetById(Expression<Func<T, bool>> ex);

        int Insert(T obj);

        int Update(T obj);

        int Delete(T obj);

        int Save();
    }
}
