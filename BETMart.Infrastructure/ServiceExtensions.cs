using System;
using System.Reflection;
using BETMart.BLL._Core;
using BETMart.BLL.Notifications;
using BETMart.BLL.Services;
using BETMart.DAL;
using BETMart.DAL.Core;
using BETMart.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BETMart.Infrastructure
{
    public static class ServiceExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>("MailSettings", a =>
            {
                a.DisplayName = configuration["MailSettings:DisplayName"];
                a.From = configuration["MailSettings:From"];
                a.Host = configuration["MailSettings:Host"];
                a.Port = Convert.ToInt32(configuration["MailSettings:Port"]);
                a.UserName = configuration["MailSettings:UserName"];
                a.Password = configuration["MailSettings:Password"];
            });
            services.AddTransient<IMailService, SMTPMailService>();
        }
        public static void AddBusinessLayer(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISettings, Settings>();
            services.AddTransient<IHttpService, HttpService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderService, OrderService>();
        }
        public static void AddPersistenceContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IBETMartContext, BETMartContext>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            #region Repositories

            services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderDetailRepository, OrderDetailRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            #endregion Repositories
        }
    }
}
