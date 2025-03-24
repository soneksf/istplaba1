using LibraryDomain.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraryInfrastructure // або LibraryInfrastructure, залежно від вашої структури проекту
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin1@gmail.com";
            string password = "Admin1_"; // Вкажіть свій пароль

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                // Створюємо нового користувача
                var admin = new User
                {
                    Email = adminEmail,
                    UserName = adminEmail
                };
                // Спроба створити користувача
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    // Додаємо роль "admin"
                    await userManager.AddToRoleAsync(admin, "admin");
                }

            }
        }

    }
}