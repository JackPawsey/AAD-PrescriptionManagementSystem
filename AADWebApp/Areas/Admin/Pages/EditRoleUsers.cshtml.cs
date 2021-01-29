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

        public string RoleName { get; private set; }

        public EditRoleUsersModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task OnGet()
        {
            var role = await _roleManager.FindByIdAsync(Request.Query["id"]);

            if (role == null)
            {
                ModelState.AddModelError("EditRoleError", "Role not found");
                return;
            }

            RoleName = role.Name;

            foreach (var user in _userManager.Users)
            {
                UserRoleModels.Add(new UserRoleModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var roleId = Request.Query["id"];
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ModelState.AddModelError("EditRoleError", "Role not found");
                return Page();
            }

            foreach (var userRoleModel in UserRoleModels)
            {
                var user = await _userManager.FindByIdAsync(userRoleModel.UserId);

                if (user == null)
                {
                    ModelState.AddModelError("EditRoleError", "No user with ID '" + userRoleModel.UserId + "' found");
                    continue;
                }

                var result = userRoleModel.IsSelected switch
                {
                    true when !await _userManager.IsInRoleAsync(user, role.Name) => await _userManager.AddToRoleAsync(user, role.Name),
                    false when await _userManager.IsInRoleAsync(user, role.Name) => await _userManager.RemoveFromRoleAsync(user, role.Name),
                    _ => null
                };

                if (result?.Succeeded == false) break;
            }

            return ModelState.ErrorCount == 0 ? (IActionResult) Redirect("/Admin/EditRole?id=" + roleId) : Page();
        }
    }
}