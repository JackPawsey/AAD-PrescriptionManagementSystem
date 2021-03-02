using AADWebApp.Areas.Identity.Data;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AADWebApp.Services.BloodTestService;

namespace AADWebApp.Areas.GeneralPractitioner.Pages
{
    [Authorize(Roles = "General Practitioner, Admin")]
    public class EnterTestResultsModel : PageModel
    {
        private readonly IBloodTestService _bloodTestService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPrescriptionService _prescriptionService;

        public List<BloodTest> BloodTests { get; private set; } = new List<BloodTest>();
        public List<BloodTestRequest> BloodTestRequests { get; private set; } = new List<BloodTestRequest>();
        public List<ApplicationUser> Patients { get; } = new List<ApplicationUser>();
        public List<Prescription> Prescriptions { get; } = new List<Prescription>();

        [BindProperty]
        public int BloodTestRequestId { get; set; }

        [BindProperty]
        public bool BloodTestResult { get; set; }

        [BindProperty]
        public DateTime BloodTestDateTime { get; set; }

        public EnterTestResultsModel(IBloodTestService bloodTestService, UserManager<ApplicationUser> userManager, IPrescriptionService prescriptionService)
        {
            _bloodTestService = bloodTestService;
            _userManager = userManager;
            _prescriptionService = prescriptionService;
        }

        public async Task OnGetAsync()
        {
            await InitPageAsync();
        }

        public async Task<PageResult> OnPostAsync()
        {
            var result = _bloodTestService.SetBloodTestResults((short) BloodTestRequestId, BloodTestResult, BloodTestDateTime);

            await InitPageAsync();

            if (result == 1)
            {
                return Page();
            }
            else
            {
                ModelState.AddModelError("Submit blood test results error", "Blood Test service returned error value");

                return Page();
            }
        }

        private async Task InitPageAsync()
        {
            BloodTests = _bloodTestService.GetBloodTests().ToList();
            BloodTestRequests = _bloodTestService.GetBloodTestRequests().Where(item => item.BloodTestStatus == BloodTestRequestStatus.Scheduled).OrderBy(item => item.BloodTestStatus).ToList();

            foreach (var item in BloodTestRequests)
            {
                Prescriptions.Add(_prescriptionService.GetPrescriptions(item.PrescriptionId).ElementAt(0));
            }

            foreach (var item in Prescriptions)
            {
                Patients.Add(await _userManager.FindByIdAsync(item.PatientId));
            }
        }
    }
}