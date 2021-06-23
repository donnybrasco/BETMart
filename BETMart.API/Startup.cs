using BETMart.DAL;
using BETMart.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.OpenApi.Models;

namespace BETMart.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            if (Configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<BETMartContext>(options => options.UseInMemoryDatabase("ConnectionString:BETMartDb"));
            }
            else
            {
                services.AddDbContext<BETMartContext>(options => options.UseSqlServer(Configuration["ConnectionString:BETMartDb"], b => b.MigrationsAssembly(typeof(BETMartContext).Assembly.FullName)));
            }
            services.AddControllers(); 
            services.AddCors();
            services.AddSwagger();
            services.AddInfrastructure(Configuration);
            services.AddAutoMapper(typeof(Startup));
            services.AddPersistenceContexts(Configuration);
            services.AddRepositories();
            services.AddBusinessLayer(Configuration); 
            services.Configure<AzureFileLoggerOptions>(Configuration.GetSection("AzureLogging"));
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "BETMart API v1");
                options.RoutePrefix = "swagger";
                options.DisplayRequestDuration();
            });
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
