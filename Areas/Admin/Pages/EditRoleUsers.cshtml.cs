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
        private readonly RoleManager<IdentityRole> RoleManager;
        private readonly UserManager<ApplicationUser> UserManager;

        [BindProperty]
        public List<UserRoleModel> userRoleModels { get; set; } = new List<UserRoleModel>();

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

        public string RoleName { get; set; }

        public EditRoleUsersModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            RoleManager = roleManager;
            UserManager = userManager;
        }

        public async Task OnGet()
        {
            var Role = await RoleManager.FindByIdAsync(Request.Query["id"]);

            RoleName = Role.Name;

            if (Role == null)
            {
                ModelState.AddModelError("", "Role not found");
                Response.Redirect("/Admin/ListRoles");
            }
            else
            {
                foreach (var user in UserManager.Users)
                {
                    var userRoleModel = new UserRoleModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };

                    if (await UserManager.IsInRoleAsync(user, Role.Name))
                    {
                        userRoleModel.IsSelected = true;
                    }
                    else
                    {
                        userRoleModel.IsSelected = false;
                    }
                    userRoleModels.Add(userRoleModel);
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var Role = await RoleManager.FindByIdAsync(Request.Query["id"]);

            if (Role == null)
            {
                ModelState.AddModelError("", "Role not found");
                Console.WriteLine("role not found");
                return Page();
            }
            else
            {
                foreach (var user in UserManager.Users)
                {
                    var userRoleModel = new UserRoleModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };

                    userRoleModels.Add(userRoleModel);
                }

                for (int x = 0; x < userRoleModels.Count; x++)
                {
                    var user = await UserManager.FindByIdAsync(userRoleModels[x].UserId);
                    IdentityResult result = null;

                    if (Input[x].IsSelected && !(await UserManager.IsInRoleAsync(user, Role.Name)))
                    {
                        result = await UserManager.AddToRoleAsync(user, Role.Name);
                    }
                    else if (!Input[x].IsSelected && await UserManager.IsInRoleAsync(user, Role.Name))
                    {
                        result = await UserManager.RemoveFromRoleAsync(user, Role.Name);
                    }

                    if (result?.Succeeded == false)
                    {
                        break;
                    }
                }

                return Redirect("/Admin/EditRole");
            }
        }
    }
}