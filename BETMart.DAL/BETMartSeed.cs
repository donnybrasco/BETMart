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
                    ProductId = 1, Name = "White Single Chair", Description = "Made in China", Price = 400, Image = GetByteArray(@"Data\Images\product_01.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product {
                    ProductId = 2, Name = "Light Blue Swivel", Description = "Made in Italy", Price = 800, Image = GetByteArray(@"Data\Images\product_03.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product {
                    ProductId = 3, Name = "White Hanging Light", Description = "Made in South Africa", Price = 300, Image = GetByteArray(@"Data\Images\product_06.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product {
                    ProductId = 4, Name = "Brown Hanging Light", Description = "Made in South Africa", Price = 450, Image = GetByteArray(@"Data\Images\product_08.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product {
                    ProductId = 5, Name = "Wooden Single Chair", Description = "Made in China", Price = 1200, Image = GetByteArray(@"Data\Images\product_07.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product {
                    ProductId = 6, Name = "3-Seater Brown Couch", Description = "Made in Turkey", Price = 2200, Image = GetByteArray(@"Data\Images\product_09.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product {
                    ProductId = 7, Name = "Decorated Brown Table", Description = "Made in China", Price = 180, Image = GetByteArray(@"Data\Images\product_10.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product {
                    ProductId = 8, Name = "3-Seater Red Couch", Description = "Made in Turkey", Price = 4300, Image = GetByteArray(@"Data\Images\product_11.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product {
                    ProductId = 9, Name = "Round Table (Small)", Description = "Made in China", Price = 150, Image = GetByteArray(@"Data\Images\product_12.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product {
                    ProductId = 10, Name = "Black Single Chair", Description = "Made in South Africa", Price = 100, Image = GetByteArray(@"Data\Images\product_14.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product {
                    ProductId = 11, Name = "White Single Couch", Description = "Made in Italy", Price = 900, Image = GetByteArray(@"Data\Images\product_15.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product {
                    ProductId = 12, Name = "Yellow Royal Double Couch", Description = "Made in Turkey", Price = 4800, Image = GetByteArray(@"Data\Images\product_16.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
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
