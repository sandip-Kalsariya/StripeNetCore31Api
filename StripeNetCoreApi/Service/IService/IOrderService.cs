using StripeNetCoreApi.DTO.RequestDTO;
using StripeNetCoreApi.DTO.ResponseDTO;
using StripeNetCoreApi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Service.IService
{
   public interface IOrderService
    {
        Task<Response<Order>> Create(int UserId, OrderRequestDTO dto);
        Response<Result<ResponseOrderDTO>> GetUserOrderList(int UserId, Pagination dto);
    }
}
