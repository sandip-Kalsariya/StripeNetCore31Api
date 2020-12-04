using StripeNetCoreApi.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Service.BasicServices
{
    public interface IBaseService : IDisposable
    {
        /// <summary>
        /// Sets current account service.
        /// </summary>
        /// <param name="accountService"></param>
        void SetAccountService(IUserService userService);
    }
}
