using System;
using System.Threading.Tasks;
using AADWebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AADWebApp.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class ListUsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public string UserId { get; set; }

        public ListUsersModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            Console.WriteLine("UserID to delete: " + UserId);

            var user = await _userManager.FindByIdAsync(UserId);

            if (user != null)
            {
                var deleteResult = await _userManager.DeleteAsync(user);

                if (deleteResult.Succeeded)
                    Response.Redirect("/Admin/ListUsers");
                else
                    foreach (var error in deleteResult.Errors)
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