using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AADWebApp.Areas.Identity.Data;
using AADWebApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;

namespace AADWebApp.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class ListUsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ISendEmailService _sendEmailService;

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public string UserId { get; set; }

        public ListUsersModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ISendEmailService sendEmailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _sendEmailService = sendEmailService;

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

        public async Task<IActionResult> OnPostDeleteUserAsync()
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

        public async Task<IActionResult> OnPostIssuePasswordResetAsync()
        {
            Console.WriteLine("UserID to issue password reset for: " + UserId);

            var user = await _userManager.FindByIdAsync(UserId);

            if (user != null)
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    null,
                    new
                    {
                        area = "Identity",
                        code
                    },
                    Request.Scheme);

                _sendEmailService.SendEmail(
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                    "Reset Password",
                    user.Email
                );

                TempData["PasswordResetSuccess"] = $"Password reset issued for {user.Email}.";
            }
            else
            {
                ModelState.AddModelError("", "User not found");
            }

            return Page();
        }
    }
}