using StripeNetCoreApi.DTO.ResponseDTO;
using StripeNetCoreApi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Service.IService
{
    public interface IAddressSerevice
    {
        Response<Address> Create(Address dto);
    }
}
