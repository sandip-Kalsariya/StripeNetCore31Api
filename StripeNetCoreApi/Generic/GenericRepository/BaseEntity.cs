using StripeNetCoreApi.Generic.IGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Generic.GenericRepository
{
    public class BaseEntity : IBaseEntity
    {
        public string DateCreated { get; set; }
        public string? DateModified { get; set; }
        public string? DateDeleted { get; set; }
    }
    public static class PagingUtils
    {
        public static IEnumerable<T> Page<T>(this IEnumerable<T> query, int pageSize, int page, out int _count, bool randomize = false)
        {
            _count = query != null ? query.Count() : 0;
            if (randomize)
                query = query.OrderBy(r => Guid.NewGuid());
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }
        public static IQueryable<T> Page<T>(this IQueryable<T> query, int pageSize, int page, out int _count, bool randomize = false)
        {
            _count = query != null ? query.Count() : 0;
  
            if (randomize)
                query = query.OrderBy(r => Guid.NewGuid());
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> query, int pageSize, int page, out int _count, int forceSkip, bool randomize = false)
        {
            _count = query != null ? query.Count() : 0;
            if (randomize)
                query = query.OrderBy(r => Guid.NewGuid());
            return query.Skip(forceSkip).Take(pageSize);
        }
        public static IEnumerable<T> Page<T>(this IEnumerable<T> query, int pageSize, int page, out int _count, int forceSkip, bool randomize = false)
        {
            _count = query != null ? query.Count() : 0;
            if (randomize)
                query = query.OrderBy(r => Guid.NewGuid());
            return query.Skip(forceSkip).Take(pageSize);
        }
    }
}
