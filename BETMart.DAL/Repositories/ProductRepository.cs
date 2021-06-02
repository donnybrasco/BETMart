using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BETMart.DAL.Core;
using BETMart.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BETMart.DAL.Repositories
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }

        Task<List<Product>> GetListAsync();
        Task<List<Product>> GetListAsync(int pageNumber, int pageSize);

        Task<Product> GetByIdAsync(int productId);

        Task<int> InsertAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(Product product);
    }
    public class ProductRepository : IProductRepository
    {
        private readonly IRepositoryAsync<Product> _repository;

        public ProductRepository(IRepositoryAsync<Product> repository)
        {
            _repository = repository;
        }

        public IQueryable<Product> Products => _repository.Entities;

        public async Task DeleteAsync(Product product)
        {
            await _repository.DeleteAsync(product);
        }

        public async Task<Product> GetByIdAsync(int productId)
        {
            return await _repository.Entities.Where(p => p.ProductId == productId).FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }
        public async Task<List<Product>> GetListAsync(int pageNumber, int pageSize)
        {
            return await _repository.GetPagedResponseAsync(pageNumber, pageSize);
        }

        public async Task<int> InsertAsync(Product product)
        {
            await _repository.AddAsync(product);
            return product.ProductId;
        }

        public async Task UpdateAsync(Product product)
        {
            await _repository.UpdateAsync(product);
        }
    }
}
