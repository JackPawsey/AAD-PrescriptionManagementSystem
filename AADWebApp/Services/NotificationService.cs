using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AADWebApp.Areas.Identity.Data;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using Microsoft.AspNetCore.Identity;
using static AADWebApp.Services.PrescriptionCollectionService;

namespace AADWebApp.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IPatientService _patientService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISendEmailService _sendEmailService;
        private readonly ISendSmsService _sendSmsService;
        private readonly IMedicationService _medicationService;

        public NotificationService(IPatientService patientService, UserManager<ApplicationUser> userManager, ISendEmailService sendEmailService, ISendSmsService sendSmsService, IMedicationService medicationService)
        {
            _patientService = patientService;
            _userManager = userManager;
            _sendEmailService = sendEmailService;
            _sendSmsService = sendSmsService;
            _medicationService = medicationService;
        }

        // Precription Notification ############################################

        public async Task SendPrescriptionNotification(Prescription prescription, int occurances, DateTime nextCollectionTime)
        {
            var patient = GetPatientRecord(prescription.PatientId);
            var medication = GetMediciationRecord(prescription.MedicationId);
            var patientAccount = await _userManager.FindByIdAsync(prescription.PatientId);

            switch (patient.CommunicationPreferences.ToString())
            {
                case "Email":
                    SendPrescriptionEmail(patientAccount, medication, prescription, occurances, nextCollectionTime);
                    break;
                case "SMS":
                    SendPrescriptionSms(patientAccount, medication, prescription, occurances, nextCollectionTime);
                    break;
                default:
                    SendPrescriptionEmail(patientAccount, medication, prescription, occurances, nextCollectionTime);
                    SendPrescriptionSms(patientAccount, medication, prescription, occurances, nextCollectionTime);
                    break;
            }
        }

        private void SendPrescriptionEmail(ApplicationUser patientAccount, Medication medication, Prescription prescription, int occurances, DateTime nextCollectionTime)
        {
            _sendEmailService.SendEmail(@$"Hello {patientAccount.FirstName}, your prescription for {medication.MedicationName} is due. <br>
                                        You will receive {occurances} more dosages of this medication that began {prescription.DateStart.ToShortDateString()} and ends {prescription.DateEnd.ToShortDateString()}. <br>
                                        The dosage is {prescription.Dosage} milligrams {prescription.IssueFrequency}. <br>
                                        Collection for the next medication is scheduled at {nextCollectionTime}",
                                        patientAccount.FirstName + " " + patientAccount.LastName + " " + medication.MedicationName + " Prescription",
                                        patientAccount.Email);
        }

        private void SendPrescriptionSms(ApplicationUser patientAccount, Medication medication, Prescription prescription, int occurances, DateTime nextCollectionTime)
        {
            var textMessage = new StringBuilder()
                .Append($"Hello {patientAccount.FirstName}, your prescription for {medication.MedicationName} is due.\n\n ")
                .Append($"You will receive {occurances} more dosages of this medication that began {prescription.DateStart.ToShortDateString()} and ends {prescription.DateEnd.ToShortDateString()}.\n\n")
                .Append($"The dosage is {prescription.Dosage} milligrams {prescription.IssueFrequency}.")
                .Append($"Collection for this medication is scheduled at {nextCollectionTime}");

            _sendSmsService.SendSms(patientAccount.PhoneNumber, textMessage.ToString());
        }

        // Collection TIme Notification ##########################################

        public async Task SendCollectionTimeNotification(Prescription prescription, DateTime collectionTime)
        {
            var patient = GetPatientRecord(prescription.PatientId);
            var medication = GetMediciationRecord(prescription.MedicationId);
            var patientAccount = await _userManager.FindByIdAsync(prescription.PatientId);

            switch (patient.CommunicationPreferences.ToString())
            {
                case "Email":
                    SendCollectionEmail(patientAccount, medication, prescription, collectionTime);
                    break;
                case "SMS":
                    SendCollectionSms(patientAccount, medication, prescription, collectionTime);
                    break;
                default:
                    SendCollectionEmail(patientAccount, medication, prescription, collectionTime);
                    SendCollectionSms(patientAccount, medication, prescription, collectionTime);
                    break;
            }
        }

        private void SendCollectionEmail(ApplicationUser patientAccount, Medication medication, Prescription prescription, DateTime collectionTime)
        {
            _sendEmailService.SendEmail(@$"Hello {patientAccount.FirstName}, collection time has been successfully updated for {medication.MedicationName} prescription. <br>
                                        The new date/time is {collectionTime}. <br>
                                        It's current status is {prescription.PrescriptionStatus}.",
                                        patientAccount.FirstName + " " + patientAccount.LastName + " " + medication.MedicationName + " Prescription Collection",
                                        patientAccount.Email);
        }

        private void SendCollectionSms(ApplicationUser patientAccount, Medication medication, Prescription prescription, DateTime collectionTime)
        {
            var textMessage = new StringBuilder()
                .Append($"Hello {patientAccount.FirstName}, collection time has been successfully updated for {medication.MedicationName} prescription.\n\n ")
                .Append($"The new date/time is {collectionTime}.\n\n")
                .Append($"It's current status is {prescription.PrescriptionStatus}.");

            _sendSmsService.SendSms(patientAccount.PhoneNumber, textMessage.ToString());
        }

        // Prescription Cancellation Notification #######################################################

        public async Task SendCancellationNotification(Prescription prescription, DateTime cancellationTime)
        {
            var patient = GetPatientRecord(prescription.PatientId);
            var medication = GetMediciationRecord(prescription.MedicationId);
            var patientAccount = await _userManager.FindByIdAsync(prescription.PatientId);

            switch (patient.CommunicationPreferences.ToString())
            {
                case "Email":
                    SendCancellationEmail(patientAccount, medication, prescription, cancellationTime);
                    break;
                case "SMS":
                    SendCancellationSms(patientAccount, medication, prescription, cancellationTime);
                    break;
                default:
                    SendCancellationEmail(patientAccount, medication, prescription, cancellationTime);
                    SendCancellationSms(patientAccount, medication, prescription, cancellationTime);
                    break;
            }
        }
        private void SendCancellationEmail(ApplicationUser patientAccount, Medication medication, Prescription prescription, DateTime cancellationTime)
        {
            _sendEmailService.SendEmail(@$"Hello {patientAccount.FirstName}, your prescription starting {prescription.DateStart} for {medication.MedicationName} has been cancelled! <br>
                                        It was cancelled at {cancellationTime}. <br>
                                        You will recieve no further medication from this prescription.",
                                        patientAccount.FirstName + " " + patientAccount.LastName + " " + medication.MedicationName + " Prescription Cancellation",
                                        patientAccount.Email);
        }

        private void SendCancellationSms(ApplicationUser patientAccount, Medication medication, Prescription prescription, DateTime cancellationTime)
        {
            var textMessage = new StringBuilder()
                .Append($"Hello {patientAccount.FirstName}, your prescription starting {prescription.DateStart} for {medication.MedicationName} has been cancelled!\n\n ")
                .Append($"It was cancelled at {cancellationTime}.")
                .Append($"You will recieve no further medication from this prescription.");

            _sendSmsService.SendSms(patientAccount.PhoneNumber, textMessage.ToString());
        }

        // Blood Test Request Notification #######################################################

        public async Task SendBloodTestRequestNotification(Prescription prescription, BloodTest bloodTest, DateTime requestTime, DateTime appointmentTime)
        {
            var patient = GetPatientRecord(prescription.PatientId);
            var medication = GetMediciationRecord(prescription.MedicationId);
            var patientAccount = await _userManager.FindByIdAsync(prescription.PatientId);

            switch (patient.CommunicationPreferences.ToString())
            {
                case "Email":
                    SendBloodTestRequestEmail(patientAccount, medication, prescription, bloodTest, requestTime, appointmentTime);
                    break;
                case "SMS":
                    SendBloodTestRequestSms(patientAccount, medication, prescription, bloodTest, requestTime, appointmentTime);
                    break;
                default:
                    SendBloodTestRequestEmail(patientAccount, medication, prescription, bloodTest, requestTime, appointmentTime);
                    SendBloodTestRequestSms(patientAccount, medication, prescription, bloodTest, requestTime, appointmentTime);
                    break;
            }
        }

        private void SendBloodTestRequestEmail(ApplicationUser patientAccount, Medication medication, Prescription prescription, BloodTest bloodTest, DateTime requestTime, DateTime appointmentTime)
        {
            _sendEmailService.SendEmail(@$"Hello {patientAccount.FirstName}, you have been requested to have a blood test for your prescription for {medication.MedicationName} starting {prescription.DateStart}. <br>
                                        The test type is: {bloodTest.FullTitle}. <br>
                                        This test was requested at {requestTime} and as scheduled to take place at {appointmentTime}. <br>
                                        Your prescription will not be approved until this test is taken and the results permit the treatment.",
                                        patientAccount.FirstName + " " + patientAccount.LastName + " Blood Test Request",
                                        patientAccount.Email);
        }

        private void SendBloodTestRequestSms(ApplicationUser patientAccount, Medication medication, Prescription prescription, BloodTest bloodTest, DateTime requestTime, DateTime appointmentTime)
        {
            var textMessage = new StringBuilder()
                .Append($"Hello {patientAccount.FirstName}, you have been requested to have a blood test for your prescription for {medication.MedicationName} starting {prescription.DateStart}.\n\n ")
                .Append($"The test type is: { bloodTest.FullTitle}.")
                .Append($"This test was requested at {requestTime} and as scheduled to take place at {appointmentTime}.")
                .Append($"Your prescription will not be approved until this test is taken and the results permit the treatment.");

            _sendSmsService.SendSms(patientAccount.PhoneNumber, textMessage.ToString());
        }

        // Blood Test Appointment Time Update #######################################################

        public async Task SendBloodTestTimeUpdateNotification(Prescription prescription, BloodTestRequest bloodTestRequest, DateTime newTime)
        {
            var patient = GetPatientRecord(prescription.PatientId);
            var medication = GetMediciationRecord(prescription.MedicationId);
            var patientAccount = await _userManager.FindByIdAsync(prescription.PatientId);

            switch (patient.CommunicationPreferences.ToString())
            {
                case "Email":
                    SendBloodTestTimeUpdateEmail(patientAccount, medication, bloodTestRequest, newTime);
                    break;
                case "SMS":
                    SendBloodTestTimeUpdateSms(patientAccount, medication, bloodTestRequest, newTime);
                    break;
                default:
                    SendBloodTestTimeUpdateEmail(patientAccount, medication, bloodTestRequest, newTime);
                    SendBloodTestTimeUpdateSms(patientAccount, medication, bloodTestRequest, newTime);
                    break;
            }
        }

        private void SendBloodTestTimeUpdateEmail(ApplicationUser patientAccount, Medication medication, BloodTestRequest bloodTestRequest, DateTime newTime)
        {
            _sendEmailService.SendEmail(@$"Hello {patientAccount.FirstName}, the blood test appointment time for your {medication.MedicationName} prescription has been updated. <br>
                                        The old time was: {bloodTestRequest.AppointmentTime}, the updated time is: {newTime}. <br>
                                        Your prescription will not be approved until this test is taken and the results permit the treatment.",
                                        patientAccount.FirstName + " " + patientAccount.LastName + " Blood Test Request",
                                        patientAccount.Email);
        }

        private void SendBloodTestTimeUpdateSms(ApplicationUser patientAccount, Medication medication, BloodTestRequest bloodTestRequest, DateTime newTime)
        {
            var textMessage = new StringBuilder()
                .Append($"Hello {patientAccount.FirstName}, the blood test appointment time for your {medication.MedicationName} prescription has been updated.\n\n ")
                .Append($"The old time was: {bloodTestRequest.AppointmentTime}, the updated time is: {newTime}.")
                .Append($"Your prescription will not be approved until this test is taken and the results permit the treatment.");

            _sendSmsService.SendSms(patientAccount.PhoneNumber, textMessage.ToString());
        }

        //#######################################################

        private Patient GetPatientRecord(string patientId)
        {
            return _patientService.GetPatients(patientId).ElementAt(0);
        }

        private Medication GetMediciationRecord(int medicationId)
        {
            return _medicationService.GetMedications((short?) medicationId).ElementAt(0);
        }
    }
}