using StripeNetCoreApi.DTO.EnumDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.DTO.RequestDTO
{
    public class OrderRequestDTO
    {

        public string Description { get; set; }
        public string DiscountCode { get; set; }
        public DiscountType DiscountType { get; set; }
        public double DiscountAmount { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public string Notes { get; set; }
    }
}
