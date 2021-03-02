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

        public List<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public List<Medication> Medications { get; set; } = new List<Medication>();
        public List<List<PrescriptionCollection>> PrescriptionCollections { get; set; } = new List<List<PrescriptionCollection>>();

        [BindProperty]
        public int prescriptionCollectionId { get; set; }

        public ProcessPrescriptionsModel(IPrescriptionService prescriptionService, IMedicationService medicationService, IPrescriptionCollectionService prescriptionCollectionService)
        {
            _prescriptionService = prescriptionService;
            _medicationService = medicationService;
            _prescriptionCollectionService = prescriptionCollectionService;
        }

        public void OnGet()
        {
            loadData();
        }

        public void OnPostPrepared()
        {
            loadData();

            var result = _prescriptionCollectionService.SetPrescriptionCollectionStatus((short) prescriptionCollectionId, CollectionStatus.BeingPrepared);

            loadData();

            if (result == 1)
            {
                TempData["EnterPrescriptionStatusSuccess"] = $"Status for Prescripton Collection {prescriptionCollectionId} was set to Being Prepared.";
            }
            else
            {
                TempData["EnterPrescriptionStatusFailure"] = $"Prescription Collection service returned error value.";
            }
        }

        public void OnPostReady()
        {
            loadData();

            var result = _prescriptionCollectionService.SetPrescriptionCollectionStatus((short) prescriptionCollectionId, CollectionStatus.CollectionReady);

            loadData();

            if (result == 1)
            {
                TempData["EnterPrescriptionStatusSuccess"] = $"Status for Prescripton Collection {prescriptionCollectionId} was set to Collection Ready.";
            }
            else
            {
                TempData["EnterPrescriptionStatusFailure"] = $"Prescription Collection service returned error value.";
            }
        }

        public void OnPostCollected()
        {
            loadData();

            var result = _prescriptionCollectionService.SetPrescriptionCollectionCollected((short) prescriptionCollectionId);

            loadData();

            if (result == 1)
            {
                TempData["EnterPrescriptionStatusSuccess"] = $"Status for Prescripton Collection {prescriptionCollectionId} was set to Collected.";
            }
            else
            {
                TempData["EnterPrescriptionStatusFailure"] = $"Prescription Collection service returned error value.";
            }
        }

        private void loadData()
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