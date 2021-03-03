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
using static AADWebApp.Services.PrescriptionService;

namespace AADWebApp.Areas.Pharmacist.Pages
{
    [Authorize(Roles = "Pharmacist, Admin")]
    public class ReviewPrescriptionsModel : PageModel
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IMedicationService _medicationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBloodTestService _bloodTestService;

        public List<Prescription> Prescriptions { get; private set; } = new List<Prescription>();
        public List<Medication> Medications { get; private set; } = new List<Medication>();
        public List<ApplicationUser> Patients { get; private set; } = new List<ApplicationUser>();
        public List<BloodTest> BloodTests { get; private set; } = new List<BloodTest>();
        public List<List<BloodTestRequest>> BloodTestRequests { get; private set; } = new List<List<BloodTestRequest>>();
        public List<List<BloodTestResult>> BloodTestResults { get; private set; } = new List<List<BloodTestResult>>();

        [BindProperty]
        public int PrescriptionId { get; set; }

        [BindProperty]
        public int BloodTestId { get; set; }

        public ReviewPrescriptionsModel(IPrescriptionService prescriptionService, IMedicationService medicationService, UserManager<ApplicationUser> userManager, IBloodTestService bloodTestService)
        {
            _prescriptionService = prescriptionService;
            _medicationService = medicationService;
            _userManager = userManager;
            _bloodTestService = bloodTestService;
        }

        public async Task OnGetAsync()
        {
            await InitPageAsync();
        }

        public async Task OnPostRequestBloodworkAsync()
        {
            await InitPageAsync();

            var statusSuccess = _prescriptionService.SetPrescriptionStatus((short)PrescriptionId, PrescriptionStatus.AwaitingBloodWork);

            var prescription = _prescriptionService.GetPrescriptions((short?)PrescriptionId).ElementAt(0);

            var requestSuccess = await _bloodTestService.RequestBloodTestAsync(prescription, (short) BloodTestId);

            await InitPageAsync();

            if (statusSuccess == 1 && requestSuccess == 1)
            {
                TempData["ReviewPrescriptionSuccess"] = $"Status for prescription {PrescriptionId} was set to Awaiting Bloodwork.";
            }
            else if (statusSuccess != 1)
            {
                TempData["ReviewPrescriptionFailure"] = $"Prescription service returned error value.";
            }
            else
            {
                TempData["ReviewPrescriptionFailure"] = $"Blood Test service returned error value.";
            }
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
            BloodTests = new List<BloodTest>();
            BloodTestRequests = new List<List<BloodTestRequest>>();
            BloodTestResults = new List<List<BloodTestResult>>();

            Prescriptions = _prescriptionService.GetPrescriptions().OrderBy(item => item.PrescriptionStatus).ToList();

            BloodTests = _bloodTestService.GetBloodTests().ToList();

            foreach (var prescription in Prescriptions)
            {
                Medications.Add(_medicationService.GetMedications(prescription.MedicationId).ElementAt(0));
                Patients.Add(await _userManager.FindByIdAsync(prescription.PatientId));

                var bloodTestRequests = _bloodTestService.GetBloodTestRequestsByPrescriptionId(prescription.Id).ToList();

                foreach (var bloodTestRequest in bloodTestRequests)
                {
                    BloodTestResults.Add(_bloodTestService.GetBloodTestResults(bloodTestRequest.Id).ToList());
                }

                BloodTestRequests.Add(bloodTestRequests);
            }

            //for (int prescription = 0; prescription < Prescriptions.Count; prescription++)
            //{
            //    Console.WriteLine("prescription id: " + Prescriptions[prescription].Id);

            //    for (int prescriptionRequests = 0; prescriptionRequests < BloodTestRequests.Count; prescriptionRequests++)
            //    {
            //        for (int bloodTestRequest = 0; bloodTestRequest < BloodTestRequests[prescriptionRequests].Count; bloodTestRequest++)
            //        {
            //            if (BloodTestRequests[prescriptionRequests].ElementAt(bloodTestRequest).PrescriptionId == Prescriptions[prescription].Id)
            //            {
            //                Console.WriteLine("prescription has blood test request with id " + BloodTestRequests[prescriptionRequests].ElementAt(bloodTestRequest).Id);

            //                for (int d = 0; d < BloodTestResults.Count; d++)
            //                {
            //                    for (int e = 0; e < BloodTestResults[d].Count; e++)
            //                    {
            //                        if (BloodTestRequests[prescriptionRequests].ElementAt(bloodTestRequest).Id == BloodTestResults[d].ElementAt(e).BloodTestRequestId)
            //                        {
            //                            Console.WriteLine("blood test result id " + BloodTestResults[d].ElementAt(e).Id);
            //                        }   
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }
    }
}