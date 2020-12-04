using StripeNetCoreApi.Generic.GenericRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Entity
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Birthdate { get; set; }
        public string Phone_Number { get; set; }
        public string Avatar { get; set; }
        public int ForgetPasswordCode { get; set; }
        public bool IsForget { get; set; }
        [ForeignKey("Roles")]
        public int? RoleId { get; set; }
        public Roles Roles { get; set; }
        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public Address Address { get; set; }
        public string Stripe_CustomerId { get; set; }
    }
}
