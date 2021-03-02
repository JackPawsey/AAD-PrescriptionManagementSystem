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

namespace AADWebApp.Areas.GeneralPractitioner.Pages
{
    [Authorize(Roles = "General Practitioner, Admin")]
    public class ScheduleBloodworkModel : PageModel
    {
        private readonly IBloodTestService _bloodTestService;
        private readonly IPrescriptionService _prescriptionService;
        private readonly IMedicationService _medicationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public List<BloodTest> BloodTests { get; set; } = new List<BloodTest>();
        public List<BloodTestRequest> BloodTestRequests { get; private set; } = new List<BloodTestRequest>();
        public List<Prescription> Prescriptions { get; } = new List<Prescription>();
        public List<Medication> Medications { get; } = new List<Medication>();
        public List<ApplicationUser> Patients { get; } = new List<ApplicationUser>();

        [BindProperty]
        public int PrescriptionId { get; set; }

        [BindProperty]
        public int BloodTestRequestId { get; set; }

        [BindProperty]
        public DateTime AppointmentDateTime { get; set; }

        public ScheduleBloodworkModel(IBloodTestService bloodTestService, IPrescriptionService prescriptionService, IMedicationService medicationService, UserManager<ApplicationUser> userManager)
        {
            _bloodTestService = bloodTestService;
            _prescriptionService = prescriptionService;
            _medicationService = medicationService;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            await InitPageAsync();
        }

        public async Task<PageResult> OnPostAsync()
        {
            var result = await _bloodTestService.SetBloodTestDateTimeAsync(Prescriptions.First(item => item.Id == PrescriptionId), (short) BloodTestRequestId, AppointmentDateTime);

            await InitPageAsync();

            if (result == 1)
            {
                return Page();
            }

            ModelState.AddModelError("Request blood test error", "Blood Test service returned error value");

            return Page();
        }

        private async Task InitPageAsync()
        {
            BloodTests = _bloodTestService.GetBloodTests().ToList();
            BloodTestRequests = _bloodTestService.GetBloodTestRequests().OrderBy(item => item.BloodTestStatus).ToList();

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