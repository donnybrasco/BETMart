using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BETMart.DAL.Core
{
    public interface IRepositoryAsync<T> 
        where T : class
    {
        IQueryable<T> Entities { get; }

        Task<T> GetByIdAsync(int id);

        Task<List<T>> GetAllAsync();

        Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize);

        Task<List<T>> FindAsync(Expression<Func<T, bool>> expr);

        Task<T> FindFirstAsync(Expression<Func<T, bool>> expr);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
    public class RepositoryAsync<T> 
        : IRepositoryAsync<T> where T : class
    {
        private readonly BETMartContext _dbContext;

        public RepositoryAsync(BETMartContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Entities => _dbContext.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext
                .Set<T>()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            return await _dbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> expr)
        {
            return await _dbContext
                                .Set<T>()
                                .Where(expr)
                                .ToListAsync();
        }
        public async Task<T> FindFirstAsync(Expression<Func<T, bool>> expr)
        {
            return await _dbContext
                        .Set<T>()
                        .FirstOrDefaultAsync(expr);
        }

        public Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).CurrentValues.SetValues(entity);
            return Task.CompletedTask;
        }
    }
}
