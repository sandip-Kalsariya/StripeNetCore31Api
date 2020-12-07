using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.DTO.RequestDTO
{
    public class PaymentRequestDTO
    {
        public int PromoOrderId { get; set; }
        public string CardId { get; set; }
        public StripeCardDTO CardDetail { get; set; }
    }
}
