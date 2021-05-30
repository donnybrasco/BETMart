using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BETMart.Common;
using BETMart.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BETMart.DAL
{
    public static class BETMartSeed
    {
        public static void InitializeSeedData(this ModelBuilder builder)
        {
            builder.Entity<Product>().HasData(new List<Product>
            {
                new Product {
                    ProductId = 1, Name = "White Single Chair", Description = "Made in China", Price = 400, Image = GetByteArray("product_01.png")
                },
                new Product {
                    ProductId = 2, Name = "Light Blue Swivel", Description = "Made in Italy", Price = 800, Image = GetByteArray("product_03.png")
                },
                new Product {
                    ProductId = 3, Name = "White Hanging Light", Description = "Made in South Africa", Price = 300, Image = GetByteArray("product_06.png")
                },
                new Product {
                    ProductId = 4, Name = "Brown Hanging Light", Description = "Made in South Africa", Price = 450, Image = GetByteArray("product_08.png")
                },
                new Product {
                    ProductId = 5, Name = "Wooden Single Chair", Description = "Made in China", Price = 1200, Image = GetByteArray("product_07.png")
                },
                new Product {
                    ProductId = 6, Name = "3-Seater Brown Couch", Description = "Made in Turkey", Price = 2200, Image = GetByteArray("product_09.png")
                },
                new Product {
                    ProductId = 7, Name = "Decorated Brown Table", Description = "Made in China", Price = 180, Image = GetByteArray("product_10.png")
                },
                new Product {
                    ProductId = 8, Name = "3-Seater Red Couch", Description = "Made in Turkey", Price = 4300, Image = GetByteArray("product_11.png")
                },
                new Product {
                    ProductId = 9, Name = "Round Table (Small)", Description = "Made in China", Price = 150, Image = GetByteArray("product_12.png")
                },
                new Product {
                    ProductId = 10, Name = "Black Single Chair", Description = "Made in South Africa", Price = 100, Image = GetByteArray("product_14.png")
                },
                new Product {
                    ProductId = 11, Name = "White Single Couch", Description = "Made in Italy", Price = 900, Image = GetByteArray("product_15.png")
                },
                new Product {
                    ProductId = 12, Name = "Yellow Royal Double Couch", Description = "Made in Turkey", Price = 4800, Image = GetByteArray("product_16.png")
                },
            });
        }

        private static byte[] GetByteArray(string path)
        {
            using var fs = new FileStream(path, FileMode.Open);
            var array = fs.ReadToEnd();

            return array;
        }
    }
}
