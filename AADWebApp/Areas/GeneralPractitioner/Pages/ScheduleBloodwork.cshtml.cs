using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AADWebApp.Areas.Identity.Data;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AADWebApp.Areas.GeneralPractitioner.Pages
{
    [Authorize(Roles = "General Practitioner, Admin")]
    public class ScheduleBloodworkModel : PageModel
    {
        private readonly IBloodTestService _bloodTestService;
        private readonly IPrescriptionService _prescriptionService;
        private readonly IMedicationService _medicationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public List<BloodTestRequest> BloodTestRequests { get; private set; }
        public List<Prescription> Prescriptions { get; private set; }
        public List<Medication> Medications { get; private set; }
        public List<ApplicationUser> Patients { get; private set; }

        [BindProperty]
        public int PrescriptionId { get; set; }

        [BindProperty]
        public int BloodTestRequestId { get; set; }

        [BindProperty]
        public DateTime AppointmentDateTime { get; set; }

        [BindProperty]
        public string SearchTerm { get; set; }

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

        public async Task OnPostAsync()
        {
            await InitPageAsync();

            var isSuccess = await _bloodTestService.SetBloodTestDateTimeAsync(Prescriptions.First(item => item.Id == PrescriptionId), (short) BloodTestRequestId, AppointmentDateTime);

            await InitPageAsync();

            if (isSuccess == 1)
            {
                TempData["ScheduleBloodworkSuccess"] = $"A blood test request {BloodTestRequestId} (prescription ID {PrescriptionId}) has been booked for {AppointmentDateTime}.";
            }
            else
            {
                TempData["ScheduleBloodworkFailure"] = $"Blood Test service returned error value.";
            }
        }

        public async Task OnPostSearchAsync()
        {
            BloodTestRequests = new List<BloodTestRequest>();
            Prescriptions = new List<Prescription>();
            Medications = new List<Medication>();
            Patients = new List<ApplicationUser>();

            var patientIds = _userManager.Users.Where(item => item.FirstName.Contains(SearchTerm) || item.LastName.Contains(SearchTerm)).Select(item => item.Id);

            Prescriptions = _prescriptionService.GetPrescriptions().Where(item => patientIds.Contains(item.PatientId)).OrderBy(item => item.Id).ToList();

            var prescriptionIds = Prescriptions.Select(item => item.Id);

            BloodTestRequests = _bloodTestService.GetBloodTestRequests().Where(item => prescriptionIds.Contains(item.PrescriptionId)).OrderBy(item => item.BloodTestStatus).ToList();

            await LoadBloodTestRequestsAndPrescriptionsAsync();
        }

        private async Task InitPageAsync()
        {
            BloodTestRequests = new List<BloodTestRequest>();
            Prescriptions = new List<Prescription>();
            Medications = new List<Medication>();
            Patients = new List<ApplicationUser>();

            BloodTestRequests = _bloodTestService.GetBloodTestRequests().OrderBy(item => item.BloodTestStatus).ToList();

            await LoadBloodTestRequestsAndPrescriptionsAsync();
        }

        private async Task LoadBloodTestRequestsAndPrescriptionsAsync()
        {
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