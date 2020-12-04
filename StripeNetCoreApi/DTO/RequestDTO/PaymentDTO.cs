using StripeNetCoreApi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.DTO.RequestDTO
{
    public class PaymentDTO
    {
        public long Amount { get; set; }
        public string Currency { get; set; }
        public string CustomerId { get; set; }
        public string CardId { get; set; }
        public string Desciption { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public Address address { get; set; }
    }
}
