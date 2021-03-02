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
    public class CancelPrescriptionModel : PageModel
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IMedicationService _medicationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public List<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public List<Medication> Medications { get; } = new List<Medication>();
        public List<ApplicationUser> Patients { get; } = new List<ApplicationUser>();

        [BindProperty]
        public int Id { get; set; }

        public CancelPrescriptionModel(IPrescriptionService prescriptionService, IMedicationService medicationService, UserManager<ApplicationUser> userManager)
        {
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
            var cancelResult = await _prescriptionService.CancelPrescriptionAsync((short) Id);

            await InitPageAsync();

            if (cancelResult == 1)
            {
                TempData["PrescriptionCancelSuccess"] = $"Prescription with ID {Id} successfully cancelled.";
            }
            else
            {
                TempData["PrescriptionCancelFailure"] = $"Prescription returned error value";
            }
        }

        private async Task InitPageAsync()
        {
            Prescriptions = _prescriptionService.GetPrescriptions().OrderBy(item => item.PrescriptionStatus).ToList();

            foreach (var item in Prescriptions)
            {
                Medications.Add(_medicationService.GetMedications(item.MedicationId).ElementAt(0));
                Patients.Add(await _userManager.FindByIdAsync(item.PatientId));
            }
        }
    }
}