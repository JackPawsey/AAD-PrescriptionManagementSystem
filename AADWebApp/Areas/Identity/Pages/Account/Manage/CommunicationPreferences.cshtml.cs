using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AADWebApp.Areas.Identity.Data;
using AADWebApp.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using static AADWebApp.Services.PatientService;

namespace AADWebApp.Areas.Identity.Pages.Account.Manage
{
    public class CommunicationPreferencesModel : PageModel
    {
        private readonly IPatientService _patientService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommunicationPreferencesModel(IPatientService patientService, UserManager<ApplicationUser> userManager)
        {
            _patientService = patientService;
            _userManager = userManager;
        }

        public CommunicationPreferences CommunicationPreference { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public SelectList CommunicationPreferences = new SelectList(
            Enum.GetValues(typeof(CommunicationPreferences))
        );

        private void InitPage(ApplicationUser user)
        {
            CommunicationPreference = _patientService.GetPatients(user.Id).First().CommunicationPreferences;

            CommunicationPreferences.First(item => item.Text == CommunicationPreference.ToString()).Selected = true;

            Input = new InputModel
            {
                NewCommunicationPreference = CommunicationPreference
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            InitPage(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            if (!ModelState.IsValid)
            {
                InitPage(user);
                return Page();
            }

            var currentCommunicationPreference = _patientService.GetPatients(user.Id).First().CommunicationPreferences;
            if (Input.NewCommunicationPreference != currentCommunicationPreference)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                _patientService.SetCommunicationPreferences(userId, Input.NewCommunicationPreference);
                StatusMessage = "Communication preferences updated/";
                return RedirectToPage();
            }

            StatusMessage = "Your communication preference is unchanged.";
            return RedirectToPage();
        }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "New communication preference")]
            public CommunicationPreferences NewCommunicationPreference { get; set; }
        }
    }
}