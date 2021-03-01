using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AADWebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AADWebApp.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class ListUsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public string UserId { get; set; }

        public ListUsersModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

            Roles = new SelectList(_roleManager.Roles.ToList());
        }

        public SelectList Roles { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Select a role for the new user")]
            public string Role { get; set; }
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
                ModelState.AddModelError("", "User not found");
            }

            return Page();
        }
    }
}