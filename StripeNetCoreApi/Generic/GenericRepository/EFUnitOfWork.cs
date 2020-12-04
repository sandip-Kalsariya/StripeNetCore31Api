using Microsoft.EntityFrameworkCore;
using StripeNetCoreApi.Generic.IGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Repository.GenericRepository
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private DbContext _context;

        public EFUnitOfWork(DbContext context)
        {
            _context = context;
        }
        public DbContext Context => _context;

        public int Insert<T>(T obj) where T : class
        {
            var _objSet = _context.Set<T>();
            _objSet.Add(obj);
            return Commit();
        }
        public async Task<int> InsertAsync<T>(T obj) where T : class
        {
            var _objSet = _context.Set<T>();
            await _objSet.AddAsync(obj);
            return await CommitAsync();
        }
        public List<T> GetAllActive<T>() where T : class
        {
            var _objSet = _context.Set<T>();
            return _objSet.ToList();
        }
        public virtual IEnumerable<T> GetwithIncludes<T>(
       Expression<Func<T, bool>> filter = null,
       Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
       string includeProperties = "") where T : class
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }


            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        public T GetByIdwithInlcudeSingle<T>(Expression<Func<T, bool>> where, string includeProperties = "") where T : class
        {
            T _objSet = _context.Set<T>().FirstOrDefault(where);
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    _objSet = _context.Set<T>().Include(includeProperty).FirstOrDefault(where);
                }
            }
            return _objSet;
        }
        public int Commit()
        {
            return _context.SaveChanges();
        }
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<List<T>> FindByConditionAsync<T>(Expression<Func<T, bool>> where) where T : class
        {
            var __objSet = _context.Set<T>();
            return await __objSet.Where(where).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync<T>() where T : class
        {
            var _objSet = _context.Set<T>();
            return await _objSet.ToListAsync();
        }

        public async Task<List<T>> GetByIdAsync<T>(Expression<Func<T, bool>> where) where T : class
        {
            var _objSet = _context.Set<T>();
            return await _objSet.Where(where).ToListAsync();
        }

        public Task<int> SaveAsyncBulk<T>(List<T> dto) where T : class
        {
            var _objSet = _context.Set<T>();
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsyncBulk<T>(List<T> dto) where T : class
        {
            var _objSet = _context.Set<T>();
            _objSet.UpdateRange(dto);
            return await CommitAsync();
        }

        public async Task<T> GetByIdAsyncSingle<T>(Expression<Func<T, bool>> where) where T : class
        {
            var _objSet = _context.Set<T>();
            return await _objSet.Include("role").FirstOrDefaultAsync(where);
        }



        public int Delete<T>(T obj, string Id) where T : class
        {
            var _objSet = _context.Set<T>();
            _objSet.Remove(obj);
            return Commit();
        }

        //public int Delete<T>(int id) where T : class
        //{
        //    var _objSet = _context.Set<T>();
        //    var obj = GetById();
        //    _objSet.Remove(obj);
        //    return Commit();
        //}

        public async Task<int> DeleteAsyncBulk<T>(List<long> dto) where T : class
        {
            var _objSet = _context.Set<T>();
            foreach (var dt in dto)
            {
                var data = await _objSet.FindAsync(long.Parse(dt.ToString()));
                await CommitAsync();
            }
            return 1;
        }

        public int Delete<T>(T obj) where T : class
        {
            var _objSet = _context.Set<T>();
            _objSet.Remove(obj);
            return Commit();
        }

        public int DeleteRow<T>(T obj) where T : class
        {
            var _objSet = _context.Set<T>();
            _objSet.Remove(obj);
            return Commit();
        }

        public int Update<T>(T obj) where T : class
        {
            var _objSet = _context.Set<T>();
            _objSet.Update(obj);
            return Commit();
        }

        public int UpdateList<T>(List<T> obj) where T : class
        {
            var _objSet = _context.Set<T>();
            _objSet.UpdateRange(obj);
            return Commit();
        }
        public int InsertList<T>(List<T> obj) where T : class
        {
            var _objSet = _context.Set<T>();
            _objSet.AddRange(obj);
            return Commit();
        }

        public List<T> GetAll<T>() where T : class
        {
            var _objSet = _context.Set<T>();
            return _objSet.ToList();
        }

        public T GetById<T>(int id) where T : class
        {
            var _objSet = _context.Set<T>();
            return _objSet.Find(id);
        }

        public T GetById<T>(Expression<Func<T, bool>> where) where T : class
        {
            var _objSet = _context.Set<T>();
            return _objSet.FirstOrDefault(where);
        }

        public async Task<T> GetByIdAsyncTest<T>(Expression<Func<T, bool>> where) where T : class
        {
            var _objSet = _context.Set<T>();
            return await _objSet.FirstOrDefaultAsync(where);
        }

        public IQueryable<T> ListQuery<T>(Expression<Func<T, bool>> where) where T : class
        {
            var _objSet = _context.Set<T>();
            return _objSet.Where(where);
        }
    }
}
