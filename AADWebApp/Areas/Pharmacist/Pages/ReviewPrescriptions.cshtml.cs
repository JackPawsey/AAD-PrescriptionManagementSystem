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
using static AADWebApp.Services.PrescriptionService;

namespace AADWebApp.Areas.Pharmacist.Pages
{
    [Authorize(Roles = "Pharmacist, Admin")]
    public class ReviewPrescriptionsModel : PageModel
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IMedicationService _medicationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public List<Prescription> Prescriptions { get; private set; } = new List<Prescription>();
        public List<Medication> Medications { get; private set; } = new List<Medication>();
        public List<ApplicationUser> Patients { get; private set; } = new List<ApplicationUser>();

        [BindProperty]
        public int PrescriptionId { get; set; }

        public ReviewPrescriptionsModel(IPrescriptionService prescriptionService, IMedicationService medicationService, UserManager<ApplicationUser> userManager)
        {
            _prescriptionService = prescriptionService;
            _medicationService = medicationService;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            await InitPageAsync();
        }

        public async Task OnPostAwaitingBloodworkAsync()
        {
            await InitPageAsync();

            var result = _prescriptionService.SetPrescriptionStatus((short) PrescriptionId, PrescriptionStatus.AwaitingBloodWork);

            await InitPageAsync();

            if (result == 1)
            {
                TempData["ReviewPrescriptionSuccess"] = $"Status for prescription {PrescriptionId} was set to Awaiting Bloodwork.";
            }
            else
            {
                TempData["ReviewPrescriptionFailure"] = $"Prescription service returned error value.";
            }
        }

        public async Task OnPostApprovedAsync()
        {
            await InitPageAsync();

            var result = _prescriptionService.SetPrescriptionStatus((short) PrescriptionId, PrescriptionStatus.Approved);

            await InitPageAsync();

            if (result == 1)
            {
                TempData["ReviewPrescriptionSuccess"] = $"Status for prescription {PrescriptionId} was set to Approved.";
            }
            else
            {
                TempData["ReviewPrescriptionFailure"] = $"Prescription service returned error value.";
            }
        }

        public async Task OnPostDeclinedAsync()
        {
            await InitPageAsync();

            var result = _prescriptionService.SetPrescriptionStatus((short) PrescriptionId, PrescriptionStatus.Declined);

            await InitPageAsync();

            if (result == 1)
            {
                TempData["ReviewPrescriptionSuccess"] = $"Status for prescription {PrescriptionId} was set to Declined.";
            }
            else
            {
                TempData["ReviewPrescriptionFailure"] = $"Prescription service returned error value.";
            }
        }

        private async Task InitPageAsync()
        {
            Prescriptions = new List<Prescription>();
            Medications = new List<Medication>();
            Patients = new List<ApplicationUser>();
    
            Prescriptions = _prescriptionService.GetPrescriptions().OrderBy(item => item.PrescriptionStatus).ToList();

            foreach (var item in Prescriptions)
            {
                Medications.Add(_medicationService.GetMedications(item.MedicationId).ElementAt(0));
                Patients.Add(await _userManager.FindByIdAsync(item.PatientId));
            }
        }
    }
}