using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BETMart.BLL._Core;
using BETMart.BLL.Models;
using BETMart.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BETMart.BLL.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<IEnumerable<Product>> GetProductsPaged(int pageNumber, int pageSize);
        Task<Product> GetProduct(int productId);
        Task AddProduct(Product obj);
        Task UpdateProduct(Product obj);
        Task DeleteProduct(int productId);
    }

    public class ProductService
        : IProductService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public ProductService(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            try
            {
                using HttpClient client = new HttpClient {BaseAddress = new Uri(_configuration["AppSettings:BETMart.API"]) };
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue(Common.Common.ContentType.Json);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userService.Token);
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = await client.GetAsync(Common.Common.APIEndpoint.Product);
                string stringData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Product>>(stringData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<Product>> GetProductsPaged(int pageNumber, int pageSize)
        {
            try
            {
                using HttpClient client = new HttpClient {BaseAddress = new Uri(_configuration["AppSettings:BETMart.API"]) };
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue(Common.Common.ContentType.Json);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userService.Token);
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = await client.GetAsync(Common.Common.APIEndpoint.Product + $"/GetPaged?pageNumber={pageNumber}&pageSize={pageSize}");
                string stringData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Product>>(stringData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Product> GetProduct(int productId)
        {
            try
            {
                using HttpClient client = new HttpClient {BaseAddress = new Uri(_configuration["AppSettings:BETMart.API"]) };
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue(Common.Common.ContentType.Json);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userService.Token);
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = await client.GetAsync(Common.Common.APIEndpoint.Product + $"/{productId}");
                string stringData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Product>(stringData);
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
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue(Common.Common.ContentType.Json);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userService.Token);
                string stringData = JsonConvert.SerializeObject(obj);
                var contentData = new StringContent(stringData, Encoding.UTF8, Common.Common.ContentType.Json);
                HttpResponseMessage response = await client.PostAsync(Common.Common.APIEndpoint.Product, contentData);
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
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue(Common.Common.ContentType.Json);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userService.Token);
                string stringData = JsonConvert.SerializeObject(obj);
                var contentData = new StringContent(stringData, Encoding.UTF8, Common.Common.ContentType.Json);
                HttpResponseMessage response = await client.PutAsync($"{Common.Common.APIEndpoint.Product}/{obj.ProductId}", contentData);
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
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue(Common.Common.ContentType.Json);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userService.Token);
                HttpResponseMessage response = await client.DeleteAsync($"{Common.Common.APIEndpoint.Product}/{productId}");
                await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
