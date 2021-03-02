using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static AADWebApp.Services.PrescriptionCollectionService;

namespace AADWebApp.Areas.Technician.Pages
{
    public class ProcessPrescriptionsModel : PageModel
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IMedicationService _medicationService;
        private readonly IPrescriptionCollectionService _prescriptionCollectionService;

        public List<Prescription> Prescriptions { get; private set; } = new List<Prescription>();
        public List<Medication> Medications { get; private set; } = new List<Medication>();
        public List<List<PrescriptionCollection>> PrescriptionCollections { get; private set; } = new List<List<PrescriptionCollection>>();

        [BindProperty]
        public int PrescriptionCollectionId { get; set; }

        public ProcessPrescriptionsModel(IPrescriptionService prescriptionService, IMedicationService medicationService, IPrescriptionCollectionService prescriptionCollectionService)
        {
            _prescriptionService = prescriptionService;
            _medicationService = medicationService;
            _prescriptionCollectionService = prescriptionCollectionService;
        }

        public void OnGet()
        {
            InitPage();
        }

        public void OnPostPrepared()
        {
            InitPage();

            var result = _prescriptionCollectionService.SetPrescriptionCollectionStatus((short) PrescriptionCollectionId, CollectionStatus.BeingPrepared);

            InitPage();

            if (result == 1)
            {
                TempData["EnterPrescriptionStatusSuccess"] = $"Status for prescription collection {PrescriptionCollectionId} was set to Being Prepared.";
            }
            else
            {
                TempData["EnterPrescriptionStatusFailure"] = $"Prescription collection service returned error value.";
            }
        }

        public void OnPostReady()
        {
            InitPage();

            var result = _prescriptionCollectionService.SetPrescriptionCollectionStatus((short) PrescriptionCollectionId, CollectionStatus.CollectionReady);

            InitPage();

            if (result == 1)
            {
                TempData["EnterPrescriptionStatusSuccess"] = $"Status for prescription collection {PrescriptionCollectionId} was set to Collection Ready.";
            }
            else
            {
                TempData["EnterPrescriptionStatusFailure"] = $"Prescription Collection service returned error value.";
            }
        }

        public void OnPostCollected()
        {
            InitPage();

            var result = _prescriptionCollectionService.SetPrescriptionCollectionCollected((short) PrescriptionCollectionId);

            InitPage();

            if (result == 1)
            {
                TempData["EnterPrescriptionStatusSuccess"] = $"Status for prescription collection {PrescriptionCollectionId} was set to Collected.";
            }
            else
            {
                TempData["EnterPrescriptionStatusFailure"] = $"Prescription collection service returned error value.";
            }
        }

        private void InitPage()
        {
            Prescriptions = new List<Prescription>();
            Medications = new List<Medication>();
            PrescriptionCollections = new List<List<PrescriptionCollection>>();

            Prescriptions = _prescriptionService.GetPrescriptions().ToList();

            foreach (var item in Prescriptions)
            {
                Medications.Add(_medicationService.GetMedications(item.MedicationId).ElementAt(0));

                PrescriptionCollections.Add(_prescriptionCollectionService.GetPrescriptionCollectionsByPrescriptionId(item.Id).ToList());
            }
        }
    }
}