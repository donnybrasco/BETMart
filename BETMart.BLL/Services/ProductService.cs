using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BETMart.BLL.Models;
using BETMart.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BETMart.BLL.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task AddProduct(Product obj);
        Task UpdateProduct(Product obj);
        Task DeleteProduct(int productId);
    }

    public class ProductService
        : IProductService
    {
        private readonly IConfiguration _configuration;

        public ProductService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            try
            {
                using HttpClient client = new HttpClient {BaseAddress = new Uri(_configuration["AppSettings:BETMart.API"]) };
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue(Infrastructure.Common.ContentType.Json);
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = await client.GetAsync(Infrastructure.Common.APIEndpoint.Product);
                string stringData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Product>>(stringData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddProduct(Product obj)
        {
            try
            {
                using HttpClient client = new HttpClient { BaseAddress = new Uri(_configuration["AppSettings:BETMart.API"]) };
                string stringData = JsonConvert.SerializeObject(obj);
                var contentData = new StringContent(stringData, Encoding.UTF8, Infrastructure.Common.ContentType.Json);
                HttpResponseMessage response = await client.PostAsync(Infrastructure.Common.APIEndpoint.Product, contentData);
                await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateProduct(Product obj)
        {
            try
            {
                using var client = new HttpClient { BaseAddress = new Uri(_configuration["AppSettings:BETMart.API"]) };
                string stringData = JsonConvert.SerializeObject(obj);
                var contentData = new StringContent(stringData, Encoding.UTF8, Infrastructure.Common.ContentType.Json);
                HttpResponseMessage response = await client.PutAsync($"{Infrastructure.Common.APIEndpoint.Product}/{obj.ProductId}", contentData);
                await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteProduct(int productId)
        {
            try
            {
                using var client = new HttpClient { BaseAddress = new Uri(_configuration["AppSettings:BETMart.API"]) };
                HttpResponseMessage response = await client.DeleteAsync($"{Infrastructure.Common.APIEndpoint.Product}/{productId}");
                await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
