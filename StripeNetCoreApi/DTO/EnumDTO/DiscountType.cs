using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.DTO.EnumDTO
{
    public enum DiscountType : short
    {
        Percentage = 1,
        Price = 2
    }
    public class ResponseDiscountType
    {
        public DiscountType Id { get; set; }
        public string Name { get; set; }
    }
}
