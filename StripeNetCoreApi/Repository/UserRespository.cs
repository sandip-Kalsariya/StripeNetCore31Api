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
    public class UserRepository : IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserRepository(IUnitOfWork context)
        {
            _unitOfWork = context;
        }
        public int Delete(User obj)
        {
            return _unitOfWork.Delete(obj);
        }

        public List<User> GetAll()
        {
            //return _unitOfWork.Get<User>(a => a.DateDeleted == null, m => m.OrderBy(x => x.Id),"Grade").ToList();
            return _unitOfWork.GetAll<User>().Where(a => a.DateDeleted == null).ToList();
        }
        public List<User> GetwithIncludes(Expression<Func<User, bool>> filter = null,
       Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null,
       string includeProperties = "")
        {
            return _unitOfWork.GetwithIncludes<User>(filter, orderBy, includeProperties).ToList();

        }
        public User GetbyidwithInclude(Expression<Func<User, bool>> filter = null, string includeProperties = "")
        {
            return _unitOfWork.GetByIdwithInlcudeSingle<User>(filter, includeProperties);

        }

        public List<User> GetAllActive()
        {
            return _unitOfWork.GetAllActive<User>();
        }

        public User GetById(Expression<Func<User, bool>> ex)
        {
            return _unitOfWork.GetById<User>(ex);
        }

        public User UserLogin(User user)
        {
            try
            {
                return _unitOfWork.ListQuery<User>(b => b.Email == user.Email && b.Password == user.Password && user.DateDeleted == null).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }


        public int Insert(User obj)
        {
            return _unitOfWork.Insert<User>(obj);
        }


        public IQueryable<User> ListQuery(Expression<Func<User, bool>> where)
        {
            return _unitOfWork.ListQuery<User>(where);
        }

        public int Save()
        {
            return _unitOfWork.Commit();
        }
        public int Update(User obj)
        {
            return _unitOfWork.Update(obj);
        }

    }
}
