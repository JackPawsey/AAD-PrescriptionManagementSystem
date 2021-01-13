using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AADWebApp.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class ListRolesModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        [BindProperty]
        public string RoleId { get; set; }

        public ListRolesModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public void OnGet()
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/");
        }

        public async Task<IActionResult> OnPost()
        {
            Console.WriteLine("Role to delete: " + RoleId);

            var role = await _roleManager.FindByIdAsync(RoleId);

            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                    Response.Redirect("/Admin/ListRoles");
                else
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
            }
            else
            {
                ModelState.AddModelError("", "Role not found");
            }

            return Page();
        }
    }
}