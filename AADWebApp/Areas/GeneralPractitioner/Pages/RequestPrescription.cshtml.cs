using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AADWebApp.Areas.Identity.Data;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using AADWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AADWebApp.Areas.GeneralPractitioner.Pages
{
    [Authorize(Roles = "General Practitioner, Admin")]
    public class RequestPrescriptionModel : PageModel
    {
        private readonly IMedicationService _medicationService;
        private readonly IPatientService _patientService;
        private readonly IPrescriptionService _prescriptionService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IEnumerable<Medication> Medications;
        public List<ApplicationUser> Patients = new List<ApplicationUser>();

        public SelectList IssueFrequencies = new SelectList(
            Enum.GetValues(typeof(PrescriptionService.IssueFrequency))
        );

        [BindProperty]
        public short MedicationId { get; set; }

        [BindProperty]
        public string Id { get; set; }

        [BindProperty]
        public int Dosage { get; set; }

        [BindProperty]
        public DateTime DateStart { get; set; }

        [BindProperty]
        public DateTime DateEnd { get; set; }

        [BindProperty]
        public PrescriptionService.IssueFrequency IssueFrequency { get; set; }

        [BindProperty]
        public string SearchTerm { get; set; }

        public RequestPrescriptionModel(IMedicationService medicationService, IPatientService patientService, IPrescriptionService prescriptionService, UserManager<ApplicationUser> userManager)
        {
            _medicationService = medicationService;
            _patientService = patientService;
            _prescriptionService = prescriptionService;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            await InitPage();
        }

        public async Task OnPostPrescribeAsync()
        {
            await InitPage();

            var isSuccess = _prescriptionService.CreatePrescription(MedicationId, Id, Dosage, DateStart, DateEnd, IssueFrequency);

            if (isSuccess == 1)
            {
                TempData["PrescriptionRequestSuccess"] = $"Prescription successfully issued, with {IssueFrequency} dosages of {Dosage}mg between {DateStart.ToShortDateString()} and {DateEnd.ToShortDateString()}.";
            }
            else
            {
                TempData["PrescriptionRequestFailure"] = $"Failed to prescribe medication.";
            }
        }

        public async Task OnPostSearchAsync()
        {
            Medications = _medicationService.GetMedications().Where(item => item.MedicationName.Contains(SearchTerm)).OrderBy(item => item.MedicationName);

            Console.WriteLine("hello world");
            
            foreach (var patient in _patientService.GetPatients())
            {
                Patients.Add(await _userManager.FindByIdAsync(patient.Id));
            }
        }

        private async Task InitPage()
        {
            Medications = _medicationService.GetMedications().OrderBy(item => item.MedicationName);

            foreach (var patient in _patientService.GetPatients())
            {
                Patients.Add(await _userManager.FindByIdAsync(patient.Id));
            }
        }
    }
}