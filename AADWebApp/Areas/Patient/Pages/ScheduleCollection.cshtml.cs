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

namespace AADWebApp.Areas.Patient.Pages
{
    [Authorize(Roles = "Patient, Authorised Carer, Admin")]
    public class ScheduleCollectionModel : PageModel
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IPrescriptionCollectionService _prescriptionCollectionService;
        private readonly IMedicationService _medicationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public List<Prescription> Prescriptions { get; private set; } = new List<Prescription>();
        public List<List<PrescriptionCollection>> PrescriptionCollections { get; private set; } = new List<List<PrescriptionCollection>>();
        public List<Medication> Medications { get; private set; } = new List<Medication>();

        [BindProperty]
        public int PrescriptionId { get; set; }

        [BindProperty]
        public DateTime CollectionDateTime { get; set; }

        public ScheduleCollectionModel(IPrescriptionService prescriptionService, IPrescriptionCollectionService prescriptionCollectionService, IMedicationService medicationService, UserManager<ApplicationUser> userManager)
        {
            _prescriptionService = prescriptionService;
            _prescriptionCollectionService = prescriptionCollectionService;
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

            var result = await _prescriptionCollectionService.SetPrescriptionCollectionTimeAsync(Prescriptions.First(item => item.Id == PrescriptionId), CollectionDateTime);

            await InitPageAsync();

            if (result == 1)
            {
                TempData["EnterPrescriptionCollectionDateTimeSuccess"] = $"Collection time for prescription {PrescriptionId} was set to {CollectionDateTime}.";
            }
            else
            {
                TempData["EnterPrescriptionCollectionDateTimeFailure"] = $"Prescription Schedule service returned error value.";
            }
        }

        private async Task InitPageAsync()
        {
            Prescriptions = new List<Prescription>();
            PrescriptionCollections = new List<List<PrescriptionCollection>>();
            Medications = new List<Medication>();

            var patientAccount = await _userManager.FindByEmailAsync(User.Identity.Name);

            Prescriptions = _prescriptionService.GetPrescriptionsByPatientId(patientAccount.Id).ToList();

            foreach (var item in Prescriptions)
            {
                Medications.Add(_medicationService.GetMedications(item.MedicationId).ElementAt(0));
                PrescriptionCollections.Add(_prescriptionCollectionService.GetPrescriptionCollectionsByPrescriptionId(item.Id).ToList());
            }
        }
    }
}