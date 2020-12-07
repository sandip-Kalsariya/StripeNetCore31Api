using StripeNetCoreApi.DTO.EnumDTO;
using StripeNetCoreApi.Generic.GenericRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Entity
{
    public class Order: BaseEntity
    {
        public int Id { get; set; }
       
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User User { get; set; }
        public string Description { get; set; }
        [ForeignKey("Card")]
        public int? CardID { get; set; }
        public Card Card { get; set; }
        public string DiscountCode { get; set; }
        public DiscountType DiscountType { get; set; }
        public double Discount { get; set; }
        public double Price { get; set; }
        public double Fee { get; set; }
        public double Net { get; set; }
        public double Charge { get; set; }
        public OrderStatus Status { get; set; }
        public string Reason_NotAbleComplete { get; set; }
        public string ChargeId { get; set; }
        public PaymentStaus? PaymentStaus { get; set; }
    }
}
