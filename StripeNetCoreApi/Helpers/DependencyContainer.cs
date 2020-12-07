using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using StripeNetCoreApi.Generic.IGeneric;
using StripeNetCoreApi.Model;
using StripeNetCoreApi.Repository;
using StripeNetCoreApi.Repository.GenericRepository;
using StripeNetCoreApi.Repository.IRepository;
using StripeNetCoreApi.Service;
using StripeNetCoreApi.Service.BasicServices;
using StripeNetCoreApi.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Helpers
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StripeDbContext>(Options => Options.UseMySql(configuration.GetConnectionString("DefaultConnection"), mySqlOptions =>
            {
                mySqlOptions.MigrationsAssembly(typeof(StripeDbContext).Assembly.FullName);
                mySqlOptions.ServerVersion(new Version(5, 7, 17), ServerType.MySql)
                .EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
            }));

            services.AddScoped<IStripeDbContext>(provider => provider.GetService<StripeDbContext>());
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUserSessionService, UserSessionService>();
            services.AddScoped<IUserSessionRepository, UserSessionRepository>();

            services.AddScoped<ICardService, CardService>();
            services.AddScoped<ICardRepository, CardRepository>();

            services.AddScoped<IAddressSerevice, AddressService>();
            services.AddScoped<IAddressRepository, AddressRepository>();

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<IUnitOfWork>(ctx => new EFUnitOfWork(ctx.GetRequiredService<StripeDbContext>()));

            services.AddScoped<BaseServices>();
            services.AddScoped<IBaseService, BaseServices>();
            return services;
        }
    }
}
