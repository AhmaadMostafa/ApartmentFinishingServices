using ApartmentFinishingServices.Core.Entities.Identity;
using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Repository.Identity.IdentitySeed
{
    public static class IdentitySeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager)
        {
            string[] roleNames = {"Customer" , "Worker" , "Admin"};
            foreach (var role in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
            }

        }
        public static async Task SeedAdminUserAsync(UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            var adminUser = await userManager.FindByEmailAsync("admin@example.com");

            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    Email = "ahmed@admin.com",
                    UserName = "admin",
                    Name = "Ahmed Mostafa",
                    CityId = 1,
                    Address = "Admin Office"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@12345");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");

                    var admin = new Admin
                    {
                        AppUserId = adminUser.Id,
                        TotalEarnings = 0
                    };

                    unitOfWork.Repository<Admin>().Add(admin);
                    await unitOfWork.CompleteAsync();
                }
            }
        }

    }
}
