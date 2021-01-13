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
    public class EditRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public RoleModel roleModel { get; set; }

        public class RoleModel
        {
            public string Id { get; set; }
            public string RoleName { get; set; }

            public List<string> Users { get; set; } = new List<string>();
        }

        public EditRoleModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            roleModel = new RoleModel();
        }

        public async Task OnGet()
        {
            var role = await _roleManager.FindByIdAsync(Request.Query["id"]);

            if (role == null)
            {
                ModelState.AddModelError("", "Role not found");
                Response.Redirect("/Admin/ListRoles");
            }
            else
            {
                roleModel.Id = role.Id;
                roleModel.RoleName = role.Name;

                foreach (var user in _userManager.Users)
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                        roleModel.Users.Add(user.UserName);
            }
        }

        public async Task OnPostAsync()
        {
            var role = await _roleManager.FindByIdAsync(Request.Query["id"]);

            if (role == null)
            {
                ModelState.AddModelError("", "Role not found");
            }
            else
            {
                role.Name = roleModel.RoleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                    Response.Redirect("/Admin/ListRoles");
                else
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
            }

            await OnGet();
        }
    }
}