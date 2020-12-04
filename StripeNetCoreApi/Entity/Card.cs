using StripeNetCoreApi.Generic.GenericRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Entity
{
    public class Card : BaseEntity  
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User User { get; set; }
        public string StripeCardId { get; set; }
    }
}
