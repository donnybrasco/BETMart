using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BETMart.Common;
using BETMart.DAL;
using BETMart.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace BETMart.Infrastructure.SeedData
{
    public static class AddBasicUser
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new User
            {
                UserName = "john.doe@abc.com",
                Email = "john.doe@abc.com",
                FirstName = "John",
                LastName = "Doe",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "P@$$w0rd!");
                    await userManager.AddToRoleAsync(defaultUser, Common.Common.Roles.Basic.ToString());
                }
            }
        }
    }
    public static class AddSuperAdminUser
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new User
            {
                UserName = "admin@betmart.co.za",
                Email = "admin@betmart.co.za",
                FirstName = "Donald",
                LastName = "Mafa",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "P@$$w0rd!");
                    await userManager.AddToRoleAsync(defaultUser, Common.Common.Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Common.Common.Roles.SuperAdmin.ToString());
                }
            }
        }
    }

    public static class AddRoles
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Common.Common.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Common.Common.Roles.Basic.ToString()));
        }
    }

    public static class AddProducts
    {
        public static async Task SeedAsync(BETMartContext db)
        {
            if (db.Products.FirstOrDefault(c => c.Name == "White Single Chair") != null) return;
            await db.Products.AddRangeAsync(new List<Product>
            {
                new Product
                {
                    Name = "White Single Chair", Description = "Made in China", Price = 400,
                    Image = GetByteArray("product_01.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product
                {
                    Name = "Light Blue Swivel", Description = "Made in Italy", Price = 800,
                    Image = GetByteArray("product_03.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product
                {
                    Name = "White Hanging Light", Description = "Made in South Africa", Price = 300,
                    Image = GetByteArray("product_06.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product
                {
                    Name = "Brown Hanging Light", Description = "Made in South Africa", Price = 450,
                    Image = GetByteArray("product_08.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product
                {
                    Name = "Wooden Single Chair", Description = "Made in China", Price = 1200,
                    Image = GetByteArray("product_07.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product
                {
                    Name = "3-Seater Brown Couch", Description = "Made in Turkey", Price = 2200,
                    Image = GetByteArray("product_09.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product
                {
                    Name = "Decorated Brown Table", Description = "Made in China", Price = 180,
                    Image = GetByteArray("product_10.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product
                {
                    Name = "3-Seater Red Couch", Description = "Made in Turkey", Price = 4300,
                    Image = GetByteArray("product_11.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product
                {
                    Name = "Round Table (Small)", Description = "Made in China", Price = 150,
                    Image = GetByteArray("product_12.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product
                {
                    Name = "Black Single Chair", Description = "Made in South Africa", Price = 100,
                    Image = GetByteArray("product_14.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product
                {
                    Name = "White Single Couch", Description = "Made in Italy", Price = 900,
                    Image = GetByteArray("product_15.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
                new Product
                {
                    Name = "Yellow Royal Double Couch", Description = "Made in Turkey", Price = 4800,
                    Image = GetByteArray("product_16.png"), CreatedBy = "SYS", CreatedDate = DateTime.Now
                },
            });

            await db.SaveChangesAsync("SYS");
        }
        
        private static byte[] GetByteArray(string fileName)
        {
            var basePath = $@"bin\Debug\net5.0\Data\Images\{fileName}";
            using var fs = new FileStream(basePath, FileMode.Open);
            return fs.ReadToEnd();
        }
    }
}
