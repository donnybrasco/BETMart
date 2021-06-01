using System;
using System.Threading;
using System.Threading.Tasks;

namespace BETMart.DAL.Core
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> Commit();
        Task<int> Commit(string userId);
        Task<int> Commit(CancellationToken cancellationToken);

        Task Rollback();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BETMartContext _dbContext;
        private bool _disposed;

        public UnitOfWork(BETMartContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<int> Commit(string userId)
        {
            return await _dbContext.SaveChangesAsync(userId);
        }
        public async Task<int> Commit()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public Task Rollback()
        {
            //todo
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                    _dbContext.Dispose();
                }
            }
            //dispose unmanaged resources
            _disposed = true;
        }
    }
}
