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
        private readonly IMedicationService _medicationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPrescriptionService _prescriptionService;

        public List<BloodTest> BloodTests { get; set; } = new List<BloodTest>();
        public List<BloodTestRequest> BloodTestRequests { get; set; } = new List<BloodTestRequest>();
        public List<Medication> Medications { get; set; } = new List<Medication>();
        public List<ApplicationUser> Patients { get; set; } = new List<ApplicationUser>();
        public List<Prescription> Prescriptions { get; set; } = new List<Prescription>();

        [BindProperty]
        public int bloodTestRequestId { get; set; }

        [BindProperty]
        public bool bloodTestResult { get; set; }

        [BindProperty]
        public DateTime bloodTestDateTime { get; set; }

        public EnterTestResultsModel(IBloodTestService bloodTestService, UserManager<ApplicationUser> userManager, IMedicationService medicationService, IPrescriptionService prescriptionService)
        {
            _bloodTestService = bloodTestService;
            _userManager = userManager;
            _medicationService = medicationService;
            _prescriptionService = prescriptionService;
        }

        public async Task OnGetAsync()
        {
            await loadData();
        }

        public async Task<PageResult> OnPostAsync()
        {
            Console.WriteLine("bloodTestRequestId " + bloodTestRequestId);
            Console.WriteLine("bloodTestResult " + bloodTestResult);
            Console.WriteLine("bloodTestDateTime " + bloodTestDateTime);

            var result = _bloodTestService.SetBloodTestResults((short) bloodTestRequestId, bloodTestResult, bloodTestDateTime);

            await loadData();

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

        private async Task loadData()
        {
            BloodTests = _bloodTestService.GetBloodTests().ToList();
            BloodTestRequests = _bloodTestService.GetBloodTestRequests().Where(item => item.BloodTestStatus == BloodTestRequestStatus.Scheduled).OrderBy(item => item.BloodTestStatus).ToList();

            foreach (var item in BloodTestRequests)
            {
                Prescriptions.Add(_prescriptionService.GetPrescriptions(item.PrescriptionId).ElementAt(0));
            }

            foreach (var item in Prescriptions)
            {
                Medications.Add(_medicationService.GetMedications(item.MedicationId).ElementAt(0));

                Patients.Add(await _userManager.FindByIdAsync(item.PatientId));
            }
        }
    }
}