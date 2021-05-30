using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BETMart.DAL
{
    public class BETMartContextFactory : IDesignTimeDbContextFactory<BETMartContext>
    {
        public BETMartContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BETMartContext>();
            optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=BETMartDb;Trusted_Connection=True;MultipleActiveResultSets=true;");

            return new BETMartContext(optionsBuilder.Options);
        }
    }
}