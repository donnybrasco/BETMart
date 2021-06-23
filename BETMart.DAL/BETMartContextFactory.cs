using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BETMart.DAL
{
    public class BETMartContextFactory : IDesignTimeDbContextFactory<BETMartContext>
    {
        private readonly IConfiguration _configuration;

        public BETMartContextFactory()
        {
            
        }
        public BETMartContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public BETMartContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BETMartContext>();
            optionsBuilder.UseSqlServer("Server=tcp:smarttech.database.windows.net,1433;Initial Catalog=betmart;Persist Security Info=False;User ID=dbadmin;Password=Mistral!@3;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            return new BETMartContext(optionsBuilder.Options);
        }
    }
}