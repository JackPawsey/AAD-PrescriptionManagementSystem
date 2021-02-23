using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AADWebApp.Areas.Identity.Data;
using AADWebApp.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace AADWebApp.Areas.Identity.Pages.Account.Manage
{
    public class EmailModel : PageModel
    {
        private readonly ISendEmailService _sendEmailService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmailModel(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ISendEmailService sendSendEmailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _sendEmailService = sendSendEmailService;
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        private async Task LoadAsync(ApplicationUser user)
        {
            var email = await _userManager.GetEmailAsync(user);
            Email = email;

            Input = new InputModel
            {
                NewEmail = email
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail != email)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmailChange",
                    null,
                    new
                    {
                        userId,
                        email = Input.NewEmail,
                        code
                    },
                    Request.Scheme);
                _sendEmailService.SendEmail(
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                    "Confirm your email",
                    Input.NewEmail);

                StatusMessage = "Confirmation link to change email sent. Please check your email.";
                return RedirectToPage();
            }

            StatusMessage = "Your email is unchanged.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                null,
                new
                {
                    area = "Identity",
                    userId,
                    code
                },
                Request.Scheme);
            _sendEmailService.SendEmail(
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                "Confirm your email",
                email);

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }
        }
    }
}