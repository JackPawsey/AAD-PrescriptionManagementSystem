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
        public List<BloodTestRequest> BloodTestRequests { get; set; } = new List<BloodTestRequest>();
        public List<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public List<Medication> Medications { get; set; } = new List<Medication>();
        public List<ApplicationUser> Patients { get; set; } = new List<ApplicationUser>();

        [BindProperty]
        public int prescriptionId { get; set; }

        [BindProperty]
        public int bloodTestRequestId { get; set; }

        [BindProperty]
        public DateTime appointmentDateTime { get; set; }

        public ScheduleBloodworkModel(IBloodTestService bloodTestService, IPrescriptionService prescriptionService, IMedicationService medicationService, UserManager<ApplicationUser> userManager)
        {
            _bloodTestService = bloodTestService;
            _prescriptionService = prescriptionService;
            _medicationService = medicationService;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            await loadData();
        }

        public async Task<PageResult> OnPostAsync()
        {
            Console.WriteLine("Prescription id " + prescriptionId);
            Console.WriteLine("blood test id " + bloodTestRequestId);
            Console.WriteLine("datetime " + appointmentDateTime);

            var result = await _bloodTestService.SetBloodTestDateTimeAsync(Prescriptions.Where(item => item.Id == prescriptionId).First(), (short) bloodTestRequestId, appointmentDateTime);

            await loadData();

            if (result == 1)
            {
                return Page();
            }
            else
            {
                ModelState.AddModelError("Request blood test error", "Blood Test service returned error value");

                return Page();
            }
        }

        private async Task loadData()
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
