using StripeNetCoreApi.DTO.EnumDTO;
using StripeNetCoreApi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.DTO.ResponseDTO
{
    public class PaymentOrderResponseDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public string Description { get; set; }
        public int? CartID { get; set; }
        public Card Card { get; set; }
        public string DiscountCode { get; set; }
        public DiscountType DiscountType { get; set; }
        public double Discount { get; set; }
        public double Price { get; set; }
        public double Fee { get; set; }
        public double Net { get; set; }
        public string Charge { get; set; }
        public OrderStatus Status { get; set; }
        public string Reason_NotAbleComplete { get; set; }
        public string ChargeId { get; set; }
        public PaymentStaus? PaymentStaus { get; set; }
    }
}
