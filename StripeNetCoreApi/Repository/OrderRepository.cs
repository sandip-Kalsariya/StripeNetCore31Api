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
    public class OrderRepository : IOrderRepository
    {
        private readonly IUnitOfWork _context;

        public OrderRepository(IUnitOfWork context)
        {
            _context = context;
        }

        public int Delete(Order obj)
        {
            return _context.Delete(obj);
        }

        public List<Order> GetAll()
        {
            return _context.GetAll<Order>().Where(a => a.DateDeleted == null).ToList();
        }

        public List<Order> GetAllActive()
        {
            return _context.GetAllActive<Order>();
        }

        public Order GetById(Expression<Func<Order, bool>> ex)
        {
            return _context.GetById<Order>(ex);
        }

        public Order GetbyidwithInclude(Expression<Func<Order, bool>> filter = null, string includeProperties = "")
        {
            return _context.GetByIdwithInlcudeSingle(filter, includeProperties);
        }

        public List<Order> GetwithIncludes(Expression<Func<Order, bool>> filter = null, Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null, string includeProperties = "")
        {
            return _context.GetwithIncludes(filter, orderBy, includeProperties).ToList();
        }

        public int Insert(Order obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<Order> ListQuery(Expression<Func<Order, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Commit();
        }

        public int Update(Order obj)
        {
            return _context.Update(obj);
        }
    }
}
