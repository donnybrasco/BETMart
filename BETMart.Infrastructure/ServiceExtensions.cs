using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using BETMart.BLL._Core;
using BETMart.BLL.Notifications;
using BETMart.BLL.Security;
using BETMart.BLL.Services;
using BETMart.DAL;
using BETMart.DAL.Core;
using BETMart.DAL.Entities;
using BETMart.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using IMailService = BETMart.BLL.Notifications.IMailService;
using MailSettings = BETMart.BLL.Notifications.MailSettings;
using SmtpMailService = BETMart.BLL.Notifications.SmtpMailService;

namespace BETMart.Infrastructure
{
    public static class ServiceExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ISettings, Settings>();
            services.AddTransient<IIdentityService, IdentityService>();

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<BETMartContext>().AddDefaultTokenProviders();
            
            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWTSettings:Issuer"],
                    ValidAudience = configuration["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                };
                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(Response.Fail("You are not Authorized"));
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(Response.Fail("You are not authorized to access this resource"));
                        return context.Response.WriteAsync(result);
                    },
                };
            });
        }

        public static void AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISettings, Settings>();
            services.AddTransient<IHttpService, HttpService>();
            services.AddTransient<IMailService, SmtpMailService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IUniqueCodeGenerator, UniqueCodeGenerator>();
            services.Configure<MailSettings>("MailSettings", a =>
            {
                a.DisplayName = configuration["MailSettings:DisplayName"];
                a.From = configuration["MailSettings:From"];
                a.Host = configuration["MailSettings:Host"];
                a.Port = Convert.ToInt32(configuration["MailSettings:Port"]);
                a.UserName = configuration["MailSettings:UserName"];
                a.Password = configuration["MailSettings:Password"];
            });
        }
        public static void AddPersistenceContexts(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<BETMartContext>(options => options.UseInMemoryDatabase("ConnectionString:BETMartDb"));
            }
            else
            {
                services.AddDbContext<BETMartContext>(options => options.UseSqlServer(configuration["ConnectionString:BETMartDb"], b => b.MigrationsAssembly(typeof(BETMartContext).Assembly.FullName)));
            }
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

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments($@"{System.AppDomain.CurrentDomain.BaseDirectory}\BETMart.API.xml");
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "BETMart API",
                    License = new OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
            });
        }
    }
}
