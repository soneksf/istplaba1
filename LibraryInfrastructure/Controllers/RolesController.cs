using LibraryInfrastructure.ViewModel;
using LibraryDomain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryInfrastructure.ViewModel;

namespace LibraryInfrastructure.Controllers
{
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private const string AllowedAdminEmail = "admin1@gmail.com";

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index() => View(_roleManager.Roles.ToList());

        public IActionResult UserList() => View(_userManager.Users.ToList());

        // GET: Roles/Edit?userId=...
        public async Task<IActionResult> Edit(string userId)
        {
            if (User.Identity?.Name?.ToLower() != AllowedAdminEmail.ToLower())
            {
                return Forbid();
            }

            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }
            return NotFound();
        }

        // POST: Roles/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, string selectedRole)
        {
            // Перевіряємо, чи поточний користувач має адресу AllowedAdminEmail
            if (User.Identity?.Name?.ToLower() != AllowedAdminEmail.ToLower())
            {
                return Forbid();
            }

            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // Отримуємо всі ролі, які були у користувача
                var userRoles = await _userManager.GetRolesAsync(user);

                // Видаляємо всі ролі
                await _userManager.RemoveFromRolesAsync(user, userRoles);

                // Додаємо лише одну обрану роль, якщо користувач вибрав її
                if (!string.IsNullOrEmpty(selectedRole))
                {
                    await _userManager.AddToRoleAsync(user, selectedRole);
                }

                return RedirectToAction("UserList");
            }
            return NotFound();
        }


        // POST: Roles/Delete (видалення ролей)
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (User.Identity?.Name?.ToLower() != AllowedAdminEmail.ToLower())
            {
                return Forbid();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        // GET: Roles/DeleteUser?userId=...
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            // Перевірка: лише користувач з AllowedAdminEmail має доступ
            if (User.Identity?.Name?.ToLower() != AllowedAdminEmail.ToLower())
            {
                return Forbid();
            }
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return View(user); // Представлення DeleteUser.cshtml для підтвердження видалення
        }

        // POST: Roles/DeleteUser (видалення користувача)
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(string userId)
        {
            if (User.Identity?.Name?.ToLower() != AllowedAdminEmail.ToLower())
            {
                return Forbid();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Забороняємо видалення "admin1@gmail.com"
            if (user.Email?.ToLower() == "admin1@gmail.com")
            {
                ModelState.AddModelError("", "Неможливо видалити головного адміністратора!");
                // Можна повернутись до списку користувачів або показати сторінку з помилкою
                return RedirectToAction("UserList");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Сталася помилка при видаленні користувача.");
                return View(user);
            }
            return RedirectToAction("UserList");
        }

    }
}
