using AADWebApp.Areas.Identity.Data;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AADWebApp.Areas.GeneralPractitioner.Pages
{
    [Authorize(Roles = "General Practitioner, Admin")]
    public class CancelPrescriptionModel : PageModel
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IMedicationService _medicationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public List<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public List<Medication> Medications { get; set; } = new List<Medication>();
        public List<ApplicationUser> Patients { get; set; } = new List<ApplicationUser>();

        [BindProperty]
        public int id { get; set; }

        public CancelPrescriptionModel(IPrescriptionService prescriptionService, IMedicationService medicationService, UserManager<ApplicationUser> userManager)
        {
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
            var result = await _prescriptionService.CancelPrescriptionAsync((short) id);

            await loadData();

            if (result == 1)
            {
                return Page();
            }
            else
            {
                ModelState.AddModelError("Cancel prescription error", "Prescription returned error value");

                return Page();
            }
        }

        private async Task loadData()
        {
            Prescriptions = _prescriptionService.GetPrescriptions().OrderBy(item=>item.PrescriptionStatus).ToList();

            foreach (var item in Prescriptions)
            {
                Medications.Add(_medicationService.GetMedications(item.MedicationId).ElementAt(0));

                Patients.Add(await _userManager.FindByIdAsync(item.PatientId));
            }
        }
    }
}