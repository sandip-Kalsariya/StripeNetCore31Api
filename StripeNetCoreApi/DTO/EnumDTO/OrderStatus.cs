using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.DTO.EnumDTO
{
    public enum OrderStatus : short
    {
        RequestAccepted = 1,
        RequestRejected,
        RequestExpired,
        RequestPending,
        CancledByInfluncer,
        CancledByBrand,
        Completed
    }
    public class ResponseOrderStatus
    {
        public OrderStatus Id { get; set; }
        public string Name { get; set; }
    }
}
