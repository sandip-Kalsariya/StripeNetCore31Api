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
    public class AddressRepository : IAddressRepository
    {
        private readonly IUnitOfWork _context;
        public AddressRepository(IUnitOfWork context)
        {
            _context = context;
        }
        public int Delete(Address obj)
        {
            return _context.Delete(obj);
        }

        public List<Address> GetAll()
        {
            return _context.GetAll<Address>().Where(a => a.DateDeleted == null).ToList();
        }

        public List<Address> GetAllActive()
        {
            return _context.GetAllActive<Address>();
        }

        public Address GetById(Expression<Func<Address, bool>> ex)
        {
            return _context.GetById<Address>(ex);
        }

        public Address GetbyidwithInclude(Expression<Func<Address, bool>> filter = null, string includeProperties = "")
        {
            return _context.GetByIdwithInlcudeSingle(filter, includeProperties);
        }

        public List<Address> GetwithIncludes(Expression<Func<Address, bool>> filter = null, Func<IQueryable<Address>, IOrderedQueryable<Address>> orderBy = null, string includeProperties = "")
        {
            return _context.GetwithIncludes(filter, orderBy, includeProperties).ToList();
        }

        public int Insert(Address obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<Address> ListQuery(Expression<Func<Address, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Commit();
        }

        public int Update(Address obj)
        {
            return _context.Update(obj);
        }
    }
}
