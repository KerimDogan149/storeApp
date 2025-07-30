using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using StoreApp.Data.Entities;
using System;
using System.Threading.Tasks;
using System.Linq;
using StoreApp.Data.Concrete;


namespace StoreApp.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = serviceProvider.GetRequiredService<StoreDbContext>();

            // Roller
            string[] roles = { "Admin", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Admin Kullanıcısı
            string adminEmail = "admin@storeapp.com";
            string adminPassword = "Admin123!";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "Kullanıcısı",
                    PhoneNumber = "5551112233",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Customer Kullanıcısı
            string customerEmail = "customer@storeapp.com";
            string customerPassword = "Customer123!";
            var customerUser = await userManager.FindByEmailAsync(customerEmail);
            if (customerUser == null)
            {
                customerUser = new AppUser
                {
                    UserName = "customer",
                    Email = customerEmail,
                    FirstName = "Müşteri",
                    LastName = "Kullanıcısı",
                    PhoneNumber = "5552223344",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(customerUser, customerPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customerUser, "Customer");
                }
            }

            // Adres ekle (varsa ekleme)
            if (!context.Addresses.Any())
            {
                context.Addresses.AddRange(
                    new Address
                    {
                        FullName = "Admin Kullanıcısı",
                        PhoneNumber = "5551112233",
                        Province = "İstanbul",
                        District = "Beşiktaş",
                        Neighborhood = "Levent",
                        FullAddress = "Admin mahallesi 1. sokak no:2",
                        Title = "Ofis",
                        AppUserId = adminUser.Id
                    },
                    new Address
                    {
                        FullName = "Müşteri Kullanıcısı",
                        PhoneNumber = "5552223344",
                        Province = "Ankara",
                        District = "Çankaya",
                        Neighborhood = "Bahçelievler",
                        FullAddress = "Müşteri sitesi B blok 5. kat",
                        Title = "Ev",
                        AppUserId = customerUser.Id
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
