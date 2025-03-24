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
    [Authorize] // Загалом доступ до ролей доступний лише авторизованим користувачам
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        // Задайте тут свою адресу адміністратора, для якої дозволено змінювати ролі
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
            // Перевіряємо, чи поточний користувач має адресу AllowedAdminEmail
            if (User.Identity?.Name?.ToLower() != AllowedAdminEmail.ToLower())
            {
                return Forbid();
            }

            // Отримуємо користувача, для якого потрібно змінити ролі
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
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            // Перевіряємо, чи поточний користувач має адресу AllowedAdminEmail
            if (User.Identity?.Name?.ToLower() != AllowedAdminEmail.ToLower())
            {
                return Forbid();
            }

            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                // Список ролей, які було додано
                var addedRoles = roles.Except(userRoles);
                // Список ролей, які було видалено
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }
            return NotFound();
        }

        // POST: Roles/Delete (для видалення ролей)
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

        // Новий метод: GET: Roles/DeleteUser?userId=...
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            // Перевірка прав адміністратора
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
            return View(user); // Створіть представлення DeleteUser.cshtml для підтвердження видалення
        }

        // Новий метод: POST: Roles/DeleteUser
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
