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
    public class CardRepository : ICardRepository
    {
        private readonly IUnitOfWork _context;
        public CardRepository(IUnitOfWork context)
        {
            _context = context;
        }
        public int Delete(Card obj)
        {
            return _context.Delete(obj);
        }

        public List<Card> GetAll()
        {
            return _context.GetAll<Card>().Where(a => a.DateDeleted == null).ToList();
        }

        public List<Card> GetAllActive()
        {
            return _context.GetAllActive<Card>();
        }

        public Card GetById(Expression<Func<Card, bool>> ex)
        {
            return _context.GetById<Card>(ex);
        }

        public Card GetbyidwithInclude(Expression<Func<Card, bool>> filter = null, string includeProperties = "")
        {
            return _context.GetByIdwithInlcudeSingle(filter, includeProperties);
        }

        public List<Card> GetwithIncludes(Expression<Func<Card, bool>> filter = null, Func<IQueryable<Card>, IOrderedQueryable<Card>> orderBy = null, string includeProperties = "")
        {
            return _context.GetwithIncludes(filter, orderBy, includeProperties).ToList();
        }

        public int Insert(Card obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<Card> ListQuery(Expression<Func<Card, bool>> where)
        {
            return _context.ListQuery(where);
        }


        public int Save()
        {
            return _context.Commit();
        }

        public int Update(Card obj)
        {
            return _context.Update(obj);
        }
    }
}
