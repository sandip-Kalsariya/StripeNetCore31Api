using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.DTO.EnumDTO
{
    public enum PaymentStaus : short
    {
        Pending = 1,
        Reject,
        Completed,
        Refund,
    }

    public class ResponsePaymentStatus
    {
        public PaymentStaus? Id { get; set; }
        public string Name { get; set; }
    }
}
