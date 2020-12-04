using StripeNetCoreApi.Entity;
using StripeNetCoreApi.Generic.IGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Repository.IRepository
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
    }
}
