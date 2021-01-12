using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AADWebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AADWebApp.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class EditRoleUsersModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public List<UserRoleModel> UserRoleModels { get; set; } = new List<UserRoleModel>();

        public class UserRoleModel
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public bool IsSelected { get; set; }
        }

        [BindProperty]
        public List<UserRoleSelected> Input { get; set; }

        public class UserRoleSelected
        {
            public bool IsSelected { get; set; }
        }

        public string RoleName { get; private set; }

        public EditRoleUsersModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task OnGet()
        {
            var role = await _roleManager.FindByIdAsync(Request.Query["id"]);

            RoleName = role.Name;

            if (role == null)
            {
                ModelState.AddModelError("", "Role not found");
                Response.Redirect("/Admin/ListRoles");
            }
            else
            {
                foreach (var user in _userManager.Users)
                {
                    var userRoleModel = new UserRoleModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };

                    if (await _userManager.IsInRoleAsync(user, role.Name))
                        userRoleModel.IsSelected = true;
                    else
                        userRoleModel.IsSelected = false;

                    UserRoleModels.Add(userRoleModel);
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var role = await _roleManager.FindByIdAsync(Request.Query["id"]);

            if (role == null)
            {
                ModelState.AddModelError("", "Role not found");
                Console.WriteLine("role not found");
                return Page();
            }
            else
            {
                foreach (var user in _userManager.Users)
                {
                    var userRoleModel = new UserRoleModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };

                    UserRoleModels.Add(userRoleModel);
                }

                for (var x = 0; x < UserRoleModels.Count; x++)
                {
                    var user = await _userManager.FindByIdAsync(UserRoleModels[x].UserId);
                    IdentityResult result = null;

                    if (Input[x].IsSelected && !await _userManager.IsInRoleAsync(user, role.Name))
                        result = await _userManager.AddToRoleAsync(user, role.Name);
                    else if (!Input[x].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                        result = await _userManager.RemoveFromRoleAsync(user, role.Name);

                    if (result?.Succeeded == false) break;
                }

                return Redirect("/Admin/EditRole");
            }
        }
    }
}