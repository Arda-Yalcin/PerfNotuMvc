using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MuzikSitesi.Models;

namespace MuzikSitesi.Data
{
    public static class DbSeeder
    {
        // Ilk kurulumda roller ve varsayilan admin hesabi olusturulur.
        public static async Task RoleEkle(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            // Gerekli roller yoksa eklenir.
            string[] roller = { "Admin", "Member" };
            foreach (var rol in roller)
            {
                if (!await roleManager.RoleExistsAsync(rol))
                {
                    await roleManager.CreateAsync(new IdentityRole(rol));
                }
            }

            // Sisteme giris icin varsayilan admin kullanici.
            var adminMail = "admin@proje.com";
            var adminUser = await userManager.FindByEmailAsync(adminMail);
            if (adminUser == null)
            {
                var newAdmin = new AppUser
                {
                    UserName = adminMail,
                    Email = adminMail,
                    Ad = "Arda",
                    Soyad = "Yonetici",
                    Adres = "Izmir",
                    Telefon = "0555 555 55 55",
                    EmailConfirmed = true
                };

                var createAdmin = await userManager.CreateAsync(newAdmin, "Admin123");
                if (createAdmin.Succeeded)
                {
                    await userManager.AddClaimAsync(newAdmin, new Claim("TamAd", newAdmin.Ad + " " + newAdmin.Soyad));
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                    Console.WriteLine("Kullanici Eklendi");
                }
            }
        }
    }
}
