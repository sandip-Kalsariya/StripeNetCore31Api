using Microsoft.EntityFrameworkCore;
using StripeNetCoreApi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StripeNetCoreApi.Helpers;

namespace StripeNetCoreApi.Model
{
    public class StripeDbContext : DbContext
    {
        public StripeDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Address> Addresses { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            #region DataSeed
            #region Roles
            builder.Entity<Roles>().HasData(
                        new Roles { Id = 1, Name = "Administrator", NormalizedName = "Admin" },
                        new Roles { Id = 2, Name = "ServiceProvider", NormalizedName = "ServiceProvider" },
                        new Roles { Id = 3, Name = "User", NormalizedName = "User" }
                    );
            #endregion

            #region User
            builder.Entity<User>().HasData(
                   new User
                   {
                       Id = 1,
                       Name = "Admin",
                       Email = "admin@localhost",
                       Password = Helper.Encrypt("Admin@123"),
                       RoleId = 1
                   });
            #endregion

            #endregion
        }
    }
}
