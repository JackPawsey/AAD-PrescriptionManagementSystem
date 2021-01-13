using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AADWebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AADWebApp.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class CreateRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateRoleModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            public string RoleName { get; set; }
        }

        public void OnGet()
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var newRole = new IdentityRole
                {
                    Name = Input.RoleName
                };

                var result = await _roleManager.CreateAsync(newRole);

                if (result.Succeeded) return Redirect("/Admin/ListRoles");

                foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
            }

            return Page();
        }
    }
}