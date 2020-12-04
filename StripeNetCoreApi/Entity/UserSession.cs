using StripeNetCoreApi.Generic.GenericRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Entity
{
    public class UserSession : BaseEntity
    {
        public int Id { get; set; }
        [ForeignKey("Users")]
        public int UserId { get; set; }
        public User User { get; set; }
        [MaxLength]
        public string AccessToken { get; set; }
        public string CreationTime { get; set; }
        public string LastModificationTime { get; set; }
    }
}
