﻿using System;
using System.Reflection;
using BETMart.BLL._Core;
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
        public static void AddBusinessLayer(this IServiceCollection services)
        {
            services.AddTransient<ISettings, Settings>();
            services.AddTransient<IHttpService, HttpService>();
            services.AddTransient<IProductService, ProductService>();
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
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            #endregion Repositories
        }
    }
}
