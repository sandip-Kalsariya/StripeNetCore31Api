using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Generic.IGeneric
{
   public interface IUnitOfWork
    {
        int Insert<T>(T obj) where T : class;
        Task<int> InsertAsync<T>(T obj) where T : class;
        int Delete<T>(T obj, string Id) where T : class;
        // int Delete<T>(int id) where T : class;
        IEnumerable<T> GetwithIncludes<T>(
       Expression<Func<T, bool>> filter = null,
       Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
       string includeProperties = "") where T : class;

        T GetByIdwithInlcudeSingle<T>(Expression<Func<T, bool>> where, string includeProperties = "") where T : class;
        Task<int> DeleteAsyncBulk<T>(List<long> dto) where T : class;
        int Delete<T>(T obj) where T : class;
        int DeleteRow<T>(T obj) where T : class;
        int Update<T>(T obj) where T : class;
        int UpdateList<T>(List<T> obj) where T : class;
        int InsertList<T>(List<T> obj) where T : class;
        List<T> GetAll<T>() where T : class;
        T GetById<T>(int id) where T : class;
        T GetById<T>(Expression<Func<T, bool>> where) where T : class;
        Task<T> GetByIdAsyncTest<T>(Expression<Func<T, bool>> where) where T : class;
        IQueryable<T> ListQuery<T>(Expression<Func<T, bool>> where) where T : class;
        List<T> GetAllActive<T>() where T : class;
        Task<List<T>> FindByConditionAsync<T>(Expression<Func<T, bool>> where) where T : class;
        Task<List<T>> GetAllAsync<T>() where T : class;
        Task<List<T>> GetByIdAsync<T>(Expression<Func<T, bool>> where) where T : class;
        Task<int> SaveAsyncBulk<T>(List<T> dto) where T : class;
        Task<int> UpdateAsyncBulk<T>(List<T> dto) where T : class;
        Task<T> GetByIdAsyncSingle<T>(Expression<Func<T, bool>> where) where T : class;
        int Commit();
        Task<int> CommitAsync();
    }
}
