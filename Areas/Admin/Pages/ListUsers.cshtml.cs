using AADWebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace AADWebApp.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class ListUsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> UserManager;

        [BindProperty]
        public string UserID { get; set; }

        public ListUsersModel(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            Console.WriteLine("UserID to delete: " + UserID);

            ApplicationUser User = await UserManager.FindByIdAsync(UserID);

            if (User != null)
            {
                var DeleteResult = await UserManager.DeleteAsync(User);

                if (DeleteResult.Succeeded)
                {
                    Response.Redirect("/Admin/ListUsers");
                }
                else
                {
                    foreach (IdentityError error in DeleteResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Role not found");
            }

            return Page();
        }
    }
}
