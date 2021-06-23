using BETMart.DAL;
using BETMart.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.AzureAppServices;

namespace BETMart
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BETMartContext>(options => options.UseSqlServer(Configuration["ConnectionString:BETMartDb"]));
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // services.AddMvc(o =>
            // {
            //     //Add Authentication to all Controllers by default.
            //     var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            //     o.Filters.Add(new AuthorizeFilter(policy));
            // });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme).AddApplicationCookie(); 
            services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/Login");
            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews()
                    .AddRazorRuntimeCompilation() //install-package Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation to get razor refresh
                    .AddNewtonsoftJson(options =>
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    );
            services.Configure<AzureFileLoggerOptions>(Configuration.GetSection("AzureLogging"));
            services.AddPersistenceContexts(Configuration);
            services.AddRepositories();
            services.AddBusinessLayer(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // app.Use(async (context, next) =>
            // {
            //     if (context.User.Identity.IsAuthenticated)
            //     {
            //         var tokenClaim = context.User.FindFirst("access_token");
            //         context.Request.Headers.Add("Authorization", "Bearer " + tokenClaim.Value);
            //     }
            //     await next();
            // });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
