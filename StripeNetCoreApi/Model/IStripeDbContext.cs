using Microsoft.EntityFrameworkCore;
using StripeNetCoreApi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Model
{
    public interface IStripeDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
