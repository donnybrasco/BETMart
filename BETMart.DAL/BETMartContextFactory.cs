using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BETMart.DAL
{
    public class BETMartContextFactory : IDesignTimeDbContextFactory<BETMartContext>
    {
        private readonly IConfiguration _configuration;

        public BETMartContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public BETMartContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BETMartContext>();
            optionsBuilder.UseSqlServer(_configuration["ConnectionString:BETMartDb"]);

            return new BETMartContext(optionsBuilder.Options);
        }
    }
}