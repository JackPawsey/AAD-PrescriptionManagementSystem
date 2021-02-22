using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AADWebApp.Areas.Identity.Data;
using AADWebApp.Interfaces;
using AADWebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace AADWebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ISendEmailService _sendEmailService;
        private readonly IPatientService _patientService;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger,
            ISendEmailService sendEmailService,
            IPatientService patientService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _sendEmailService = sendEmailService;
            _patientService = patientService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public SelectList CommunicationPreferences = new SelectList(
            Enum.GetValues(typeof(PatientService.CommunicationPreferences))
        );

        public string ReturnUrl { get; private set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        private string DefaultRole { get; set; } = "Patient";

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "City")]
            public string City { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "NHS Number")]
            public string NHSNumber { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "General Practitioner")]
            public string GeneralPractitioner { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Communication Preference")]
            public PatientService.CommunicationPreferences CommunicationPreference { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("Admin")) Response.Redirect("/");

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    City = Input.City,
                    PhoneNumber = Input.PhoneNumber,
                    NHSNumber = Input.NHSNumber,
                    GeneralPractioner = Input.GeneralPractitioner
                };
                var createResult = await _userManager.CreateAsync(user, Input.Password);
                var createPatientRecord = _patientService.CreateNewPatientEntry(user.Id, Input.CommunicationPreference, Input.NHSNumber, Input.GeneralPractitioner);

                var addToRoleResult = new IdentityResult();

                if (await _roleManager.RoleExistsAsync(DefaultRole))
                {
                    addToRoleResult = await _userManager.AddToRoleAsync(user, DefaultRole);

                    if (!addToRoleResult.Succeeded)
                        foreach (var error in addToRoleResult.Errors)
                            ModelState.AddModelError("", error.Description);
                }
                else
                {
                    ModelState.AddModelError("", "Default role does not exist");
                }

                if (createResult.Succeeded && addToRoleResult.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        null,
                        new
                        {
                            area = "Identity",
                            userId = user.Id,
                            code = code,
                            returnUrl = returnUrl
                        },
                        Request.Scheme);

                    _sendEmailService.SendEmail($"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.", "Confirm your email", Input.Email);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new
                        {
                            email = Input.Email,
                            returnUrl = returnUrl
                        });
                    }
                    else
                    {
                        if (User.IsInRole("Admin"))
                        {
                            Response.Redirect("/Admin/ListUsers");
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, false);
                            return LocalRedirect(returnUrl);
                        }
                    }
                }

                if (createPatientRecord != 1)
                {
                    _logger.LogError("Failed to create a patient record in the patients table.");
                }

                foreach (var error in createResult.Errors) ModelState.AddModelError(string.Empty, error.Description);
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}