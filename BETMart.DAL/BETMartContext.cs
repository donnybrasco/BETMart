using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BETMart.DAL.Core;
using BETMart.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BETMart.DAL
{
    public interface IBETMartContext
    {
        int SaveChanges(string userId = null);
        int SaveChanges(bool acceptAllChangesOnSuccess, string userId = null);
        Task<int> SaveChangesAsync(string userId = null);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken(), string userId = null);
        //-----
        DbSet<User> Users { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Address> Addresses { get; set; }
        DbSet<BillingInformation> BillingInformations { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderDetail> OrderDetails { get; set; }
    }
    public class BETMartContext
        : DbContext
        , IBETMartContext
    {
        #region Ctor

        public BETMartContext(DbContextOptions<BETMartContext> opts)
            : base(opts)
        {
    }

        #endregion

        #region OnModelCreating

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.InitializeSeedData();
        }

        #endregion

        #region DbSets

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<BillingInformation> BillingInformations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        #endregion

        #region SaveChanges

        public virtual int SaveChanges(string userId = null)
        {
            try
            {
                SetAudit(userId);

                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual int SaveChanges(bool acceptAllChangesOnSuccess, string userId = null)
        {
            SetAudit(userId);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public virtual Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken(), string userId = null)
        {
            SetAudit(userId);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public virtual Task<int> SaveChangesAsync(string userId = null)
        {
            SetAudit(userId);
            return base.SaveChangesAsync();
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken, string userId = null)
        {
            try
            {
                SetAudit(userId);

                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Set Audit

        private void SetAudit(string userId)
        {
            var addedAuditedEntities = ChangeTracker.Entries<EntityBase>().Where(p => p.State == EntityState.Added)
                .Select(p => p.Entity);
            var modifiedAuditedEntities = ChangeTracker.Entries<EntityBase>().Where(p => p.State == EntityState.Modified)
                .Select(p => p.Entity);
            var now = DateTime.UtcNow;
            foreach (var added in addedAuditedEntities)
            {
                added.CreatedDate = now;
                added.CreatedBy = userId;
                added.UpdatedDate = now;
                added.UpdatedBy = userId;
            }

            foreach (var modified in modifiedAuditedEntities)
            {
                if (modified.CreatedDate == DateTime.MinValue)
                    modified.CreatedDate = now;
                if (string.IsNullOrEmpty(modified.CreatedBy))
                    modified.CreatedBy = userId;
                modified.UpdatedDate = now;
                modified.UpdatedBy = userId;
            }
        }

        #endregion
    }
}
