using StripeNetCoreApi.Entity;
using StripeNetCoreApi.Generic.IGeneric;
using StripeNetCoreApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Repository
{
    public class UserSessionRepository : IUserSessionRepository
    {
        private readonly IUnitOfWork _context;
        public UserSessionRepository(IUnitOfWork context)
        {
            _context = context;
        }
        public int Delete(UserSession obj)
        {
            return _context.Delete(obj);
        }
        public List<UserSession> GetAll()
        {
            return _context.GetAll<UserSession>().Where(a => a.DateDeleted == null).ToList();
        }
        public List<UserSession> GetAllActive()
        {
            return _context.GetAllActive<UserSession>();
        }
        public UserSession GetById(Expression<Func<UserSession, bool>> ex)
        {
            return _context.GetById(ex);
        }
        public int Insert(UserSession obj)
        {
            return _context.Insert(obj);
        }
        public IQueryable<UserSession> ListQuery(Expression<Func<UserSession, bool>> where)
        {
            return _context.ListQuery(where);
        }
        public int Save()
        {
            return _context.Commit();
        }
        public int Update(UserSession obj)
        {
            return _context.Update(obj);
        }
        public UserSession GetUserSessionByAccessToken(string token)
        {
            return _context.GetById<UserSession>(f => f.AccessToken == token);
        }
        public List<UserSession> GetwithIncludes(Expression<Func<UserSession, bool>> filter = null, Func<IQueryable<UserSession>, IOrderedQueryable<UserSession>> orderBy = null, string includeProperties = "")
        {
            return _context.GetwithIncludes(filter, orderBy, includeProperties).ToList();
        }
        public UserSession GetbyidwithInclude(Expression<Func<UserSession, bool>> filter = null, string includeProperties = "")
        {
            return _context.GetByIdwithInlcudeSingle(filter, includeProperties);
        }
    }
}
