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
    public class EditRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> RoleManager;
        private readonly UserManager<ApplicationUser> UserManager;

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
            RoleManager = roleManager;
            UserManager = userManager;
            roleModel = new RoleModel();
        }

        public async Task OnGet()
        {
            var Role = await RoleManager.FindByIdAsync(Request.Query["id"]);
            
            if (Role == null)
            {
                ModelState.AddModelError("", "Role not found");
                Response.Redirect("/Admin/ListRoles");
            }
            else
            {
                this.roleModel.Id = Role.Id;
                this.roleModel.RoleName = Role.Name;

                foreach (var User in UserManager.Users)
                {
                    if (await UserManager.IsInRoleAsync(User, Role.Name))
                    {
                        this.roleModel.Users.Add(User.UserName);
                    }
                }
            }
        }

        public async Task OnPostAsync()
        {
            var Role = await RoleManager.FindByIdAsync(Request.Query["id"]);

            if (Role == null)
            {
                ModelState.AddModelError("", "Role not found");
            }
            else
            {
                Role.Name = roleModel.RoleName;
                var result = await RoleManager.UpdateAsync(Role);

                if (result.Succeeded)
                {
                    Response.Redirect("/Admin/ListRoles");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            await OnGet();
        }
    }
}
