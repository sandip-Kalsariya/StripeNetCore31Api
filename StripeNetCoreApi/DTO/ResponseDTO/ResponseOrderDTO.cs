using StripeNetCoreApi.DTO.EnumDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.DTO.ResponseDTO
{
    public class ResponseOrderDTO
    {
        public int OrderId { get; set; }
        public string ServiceProviderUserName { get; set; }
        public int? ServiceProviderUserId { get; set; }
        public string CreatedDate { get; set; }
        public string ExpireTime { get; set; }
        public double Amount { get; set; }
        public double Discount { get; set; }
        public ResponseOrderStatus Status { get; set; }
        public ResponsePaymentStatus? PaymentStaus { get; set; }
    }
}
