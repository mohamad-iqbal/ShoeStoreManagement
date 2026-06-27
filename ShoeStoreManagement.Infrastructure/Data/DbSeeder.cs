using ShoeStoreManagement.Domain.Entities;
using ShoeStoreManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var store = new Store
            {
                Name = "Main Store"
            };

            context.Stores.Add(store);

            await context.SaveChangesAsync();

            var admin = new User
            {
                FullName = "Admin",
                Email = "admin@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"),
                Role = Role.Admin,
                StoreId = store.Id
            };

            context.Users.Add(admin);

            await context.SaveChangesAsync();
        }
    }
}
