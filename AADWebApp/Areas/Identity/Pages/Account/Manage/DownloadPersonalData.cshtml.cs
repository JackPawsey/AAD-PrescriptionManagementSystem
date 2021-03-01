using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AADWebApp.Areas.Identity.Data;
using AADWebApp.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AADWebApp.Areas.Identity.Pages.Account.Manage
{
    public class DownloadPersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DownloadPersonalDataModel> _logger;
        private readonly IPatientService _patientService;
        private readonly IPrescriptionService _prescriptionService;
        private readonly IPrescriptionCollectionService _prescriptionCollectionService;
        private readonly IMedicationService _medicationService;
        private readonly IBloodTestService _bloodTestService;

        public DownloadPersonalDataModel(UserManager<ApplicationUser> userManager, ILogger<DownloadPersonalDataModel> logger, IPatientService patientService, IPrescriptionService prescriptionService, IPrescriptionCollectionService prescriptionCollectionService, IMedicationService medicationService, IBloodTestService bloodTestService)
        {
            _userManager = userManager;
            _logger = logger;
            _patientService = patientService;
            _prescriptionService = prescriptionService;
            _prescriptionCollectionService = prescriptionCollectionService;
            _medicationService = medicationService;
            _bloodTestService = bloodTestService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("User with ID '{UserId}' asked for their personal data.", _userManager.GetUserId(User));

            // Only include personal data for download
            var personalData = new Dictionary<string, object>();
            var personalDataProps = typeof(ApplicationUser).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
            foreach (var p in personalDataProps)
            {
                personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            switch (userRoles)
            {
                case var i when userRoles.Contains("Patient"):
                    // Patients data
                    var patientResults = _patientService.GetPatients(user.Id).ToList();
                    if (patientResults.Count() == 1)
                    {
                        var patientData = new Dictionary<string, string>();

                        var patient = patientResults.First();
                        patientData.Add("CommunicationPreferences", patient.CommunicationPreferences.ToString());
                        patientData.Add("NhsNumber", patient.NhsNumber);
                        patientData.Add("GeneralPractitioner", patient.GeneralPractitioner);

                        personalData.Add("PatientData", patientData);
                    }
                    else
                    {
                        ModelState.AddModelError("", "More than one patient found");
                        return StatusCode(500);
                    }

                    // Prescriptions data
                    var prescriptionResults = _prescriptionService.GetPrescriptionsByPatientId(user.Id);
                    var prescriptions = prescriptionResults.ToList();
                    if (prescriptions.Any())
                    {
                        var prescriptionsData = new Collection<object>();

                        foreach (var prescription in prescriptions)
                        {
                            var prescriptionData = new Dictionary<string, object>();

                            prescriptionData.Add("Id", prescription.Id);
                            prescriptionData.Add("MedicationId", prescription.MedicationId);

                            // Get the medication data
                            var medication = _medicationService.GetMedications(prescription.MedicationId);
                            var medications = medication.ToList();
                            if (medications.Count() == 1)
                            {
                                prescriptionData.Add("MedicationName", medications.First().MedicationName);
                                prescriptionData.Add("BloodWorkRestrictionLevel", medications.First().BloodWorkRestrictionLevel);
                            }

                            // Prescription data continued
                            prescriptionData.Add("Dosage", prescription.Dosage);
                            prescriptionData.Add("DateStart", prescription.DateStart);
                            prescriptionData.Add("DateEnd", prescription.DateEnd);
                            prescriptionData.Add("PrescriptionStatus", prescription.PrescriptionStatus.ToString());
                            prescriptionData.Add("IssueFrequency", prescription.IssueFrequency.ToString());

                            // Get prescription collection history
                            var prescriptionCollectionResults = _prescriptionCollectionService.GetPrescriptionCollectionsByPrescriptionId(prescription.Id);
                            if (prescriptionCollectionResults.Any()) prescriptionData.Add("Collections", prescriptionCollectionResults);

                            // Get blood test request history
                            var bloodTestRequestsResults = _bloodTestService.GetBloodTestRequests(prescription.Id);
                            var bloodTestRequests = bloodTestRequestsResults.ToList();
                            if (bloodTestRequests.Any())
                            {
                                var bloodTestRequestData = new Dictionary<string, object>();

                                foreach (var bloodTestRequest in bloodTestRequests)
                                {
                                    bloodTestRequestData.Add("Id", bloodTestRequest.Id);
                                    bloodTestRequestData.Add("BloodTestId", bloodTestRequest.BloodTestId);

                                    // Get blood test information
                                    var bloodTests = _bloodTestService.GetBloodTests(bloodTestRequest.BloodTestId);
                                    var bloodTest = bloodTests.ToList();
                                    if (bloodTest.Count() == 1) bloodTestRequestData.Add("BloodTestInformation", bloodTest.First());

                                    // Blood test request continued
                                    bloodTestRequestData.Add("AppointmentTime", bloodTestRequest.AppointmentTime);

                                    // Get blood test results
                                    var bloodTestResultsResult = _bloodTestService.GetBloodTestResults(bloodTestRequest.Id);
                                    if (bloodTestResultsResult.Any()) bloodTestRequestData.Add("Results", bloodTestResultsResult);
                                }

                                prescriptionData.Add("Blood Test Requests", bloodTestRequestData);
                            }

                            prescriptionsData.Add(prescriptionData);
                        }

                        personalData.Add("Prescriptions", prescriptionsData);
                    }

                    break;
            }

            var logins = await _userManager.GetLoginsAsync(user);
            foreach (var l in logins)
            {
                personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
            }

            Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");

            var serializedPersonalData = Encoding.UTF8.GetBytes(
                JsonConvert.SerializeObject(personalData, new JsonSerializerSettings
                {
                    StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
                })
            );

            return new FileContentResult(serializedPersonalData, "application/json");
        }
    }
}