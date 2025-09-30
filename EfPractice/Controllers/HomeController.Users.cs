using EfPractice.Models;
using EfPractice.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace EfPractice.Controllers
{
    public partial class HomeController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Users(string? editId)
        {
            var users = await _master.GetAllUsersAsync();
            UserEditViewModel? editUser = null;
            if (!string.IsNullOrEmpty(editId))
            {
                var u = users.FirstOrDefault(x => x.Id == editId);
                if (u != null)
                {
                    editUser = new UserEditViewModel
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                        Email = u.Email,
                        RoleName = u.UserRoleId == 1 ? "Admin" : u.UserRoleId == 2 ? "Manager" : u.UserRoleId == 3 ? "Staff" : "User"
                    };
                }
            }
            var vm = new UserListEditViewModel
            {
                Users = users,
                EditUser = editUser ?? new UserEditViewModel()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SaveUser(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var users = await _master.GetAllUsersAsync();
                var vm = new UserListEditViewModel { Users = users, EditUser = model };
                ViewData["ShowValidationSummary"] = true;
                return View("Users", vm);
            }
            if (string.IsNullOrEmpty(model.Id))
            {
                await _master.AddUserAsync(model);
            }
            else
            {
                await _master.UpdateUserAsync(model);
            }
            return RedirectToAction("Users");
        }
    }
}
