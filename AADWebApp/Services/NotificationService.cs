using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AADWebApp.Areas.Identity.Data;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using Microsoft.AspNetCore.Identity;
using static AADWebApp.Services.PatientService;

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

        public async Task<bool> SendPrescriptionNotification(Prescription prescription, int occurrences, DateTime nextCollectionTime)
        {
            var patient = GetPatientRecord(prescription.PatientId);
            var medication = GetMedicationRecord(prescription.MedicationId);
            var patientAccount = await _userManager.FindByIdAsync(prescription.PatientId);

            switch (patient.CommunicationPreferences)
            {
                case CommunicationPreferences.Email:
                    return SendPrescriptionEmail(patientAccount, medication, prescription, occurrences, nextCollectionTime);
                case CommunicationPreferences.Sms:
                    return SendPrescriptionSms(patientAccount, medication, prescription, occurrences, nextCollectionTime);
                case CommunicationPreferences.Both:
                    var result1 = SendPrescriptionEmail(patientAccount, medication, prescription, occurrences, nextCollectionTime);
                    var result2 = SendPrescriptionSms(patientAccount, medication, prescription, occurrences, nextCollectionTime);

                    return result1 & result2;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool SendPrescriptionEmail(ApplicationUser patientAccount, Medication medication, Prescription prescription, int occurrences, DateTime nextCollectionTime)
        {
            return _sendEmailService.SendEmail(@$"Hello {patientAccount.FirstName}, your prescription for {medication.MedicationName} is due. <br>
                                        You will receive {occurrences} more dosages of this medication that began {prescription.DateStart.ToShortDateString()} and ends {prescription.DateEnd.ToShortDateString()}. <br>
                                        The dosage is {prescription.Dosage} milligrams {prescription.IssueFrequency}. <br>
                                        Collection for the next medication is scheduled at {nextCollectionTime}",
                                        patientAccount.FirstName + " " + patientAccount.LastName + " " + medication.MedicationName + " Prescription",
                                        patientAccount.Email);
        }

        private bool SendPrescriptionSms(ApplicationUser patientAccount, Medication medication, Prescription prescription, int occurrences, DateTime nextCollectionTime)
        {
            var textMessage = new StringBuilder()
                .Append($"Hello {patientAccount.FirstName}, your prescription for {medication.MedicationName} is due.\n\n ")
                .Append($"You will receive {occurrences} more dosages of this medication that began {prescription.DateStart.ToShortDateString()} and ends {prescription.DateEnd.ToShortDateString()}.\n\n")
                .Append($"The dosage is {prescription.Dosage} milligrams {prescription.IssueFrequency}.")
                .Append($"Collection for this medication is scheduled at {nextCollectionTime}");

            return _sendSmsService.SendSms(patientAccount.PhoneNumber, textMessage.ToString());
        }

        // Collection TIme Notification ##########################################

        public async Task<bool> SendCollectionTimeNotification(Prescription prescription, DateTime collectionTime)
        {
            var patient = GetPatientRecord(prescription.PatientId);
            var medication = GetMedicationRecord(prescription.MedicationId);
            var patientAccount = await _userManager.FindByIdAsync(prescription.PatientId);

            switch (patient.CommunicationPreferences)
            {
                case CommunicationPreferences.Email:
                    return SendCollectionEmail(patientAccount, medication, prescription, collectionTime);
                case CommunicationPreferences.Sms:
                    return SendCollectionSms(patientAccount, medication, prescription, collectionTime);
                case CommunicationPreferences.Both:
                    var result1 = SendCollectionEmail(patientAccount, medication, prescription, collectionTime);
                    var result2 = SendCollectionSms(patientAccount, medication, prescription, collectionTime);

                    return result1 & result2;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool SendCollectionEmail(ApplicationUser patientAccount, Medication medication, Prescription prescription, DateTime collectionTime)
        {
            return _sendEmailService.SendEmail(@$"Hello {patientAccount.FirstName}, collection time has been successfully updated for {medication.MedicationName} prescription. <br>
                                        The new date/time is {collectionTime}. <br>
                                        It's current status is {prescription.PrescriptionStatus}.",
                                        patientAccount.FirstName + " " + patientAccount.LastName + " " + medication.MedicationName + " Prescription Collection",
                                        patientAccount.Email);
        }

        private bool SendCollectionSms(ApplicationUser patientAccount, Medication medication, Prescription prescription, DateTime collectionTime)
        {
            var textMessage = new StringBuilder()
                .Append($"Hello {patientAccount.FirstName}, collection time has been successfully updated for {medication.MedicationName} prescription.\n\n ")
                .Append($"The new date/time is {collectionTime}.\n\n")
                .Append($"It's current status is {prescription.PrescriptionStatus}.");

            return _sendSmsService.SendSms(patientAccount.PhoneNumber, textMessage.ToString());
        }

        // Prescription Cancellation Notification #######################################################

        public async Task<bool> SendCancellationNotification(Prescription prescription, DateTime cancellationTime)
        {
            var patient = GetPatientRecord(prescription.PatientId);
            var medication = GetMedicationRecord(prescription.MedicationId);
            var patientAccount = await _userManager.FindByIdAsync(prescription.PatientId);

            switch (patient.CommunicationPreferences)
            {
                case CommunicationPreferences.Email:
                    return SendCancellationEmail(patientAccount, medication, prescription, cancellationTime);
                case CommunicationPreferences.Sms:
                    return SendCancellationSms(patientAccount, medication, prescription, cancellationTime);
                case CommunicationPreferences.Both:
                    var result1 = SendCancellationEmail(patientAccount, medication, prescription, cancellationTime);
                    var result2 = SendCancellationSms(patientAccount, medication, prescription, cancellationTime);

                    return result1 & result2;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool SendCancellationEmail(ApplicationUser patientAccount, Medication medication, Prescription prescription, DateTime cancellationTime)
        {
            return _sendEmailService.SendEmail(@$"Hello {patientAccount.FirstName}, your prescription starting {prescription.DateStart} for {medication.MedicationName} has been cancelled! <br>
                                        It was cancelled at {cancellationTime}. <br>
                                        You will receive no further medication from this prescription.",
                                        patientAccount.FirstName + " " + patientAccount.LastName + " " + medication.MedicationName + " Prescription Cancellation",
                                        patientAccount.Email);
        }

        private bool SendCancellationSms(ApplicationUser patientAccount, Medication medication, Prescription prescription, DateTime cancellationTime)
        {
            var textMessage = new StringBuilder()
                .Append($"Hello {patientAccount.FirstName}, your prescription starting {prescription.DateStart} for {medication.MedicationName} has been cancelled!\n\n ")
                .Append($"It was cancelled at {cancellationTime}.")
                .Append($"You will receive no further medication from this prescription.");

            return _sendSmsService.SendSms(patientAccount.PhoneNumber, textMessage.ToString());
        }

        // Blood Test Request Notification #######################################################

        public async Task<bool> SendBloodTestRequestNotification(Prescription prescription, BloodTest bloodTest, DateTime requestTime, DateTime appointmentTime)
        {
            var patient = GetPatientRecord(prescription.PatientId);
            var medication = GetMedicationRecord(prescription.MedicationId);
            var patientAccount = await _userManager.FindByIdAsync(prescription.PatientId);

            switch (patient.CommunicationPreferences)
            {
                case CommunicationPreferences.Email:
                    return SendBloodTestRequestEmail(patientAccount, medication, prescription, bloodTest, requestTime, appointmentTime);
                case CommunicationPreferences.Sms:
                    return SendBloodTestRequestSms(patientAccount, medication, prescription, bloodTest, requestTime, appointmentTime);
                case CommunicationPreferences.Both:
                    var result1 = SendBloodTestRequestEmail(patientAccount, medication, prescription, bloodTest, requestTime, appointmentTime);
                    var result2 = SendBloodTestRequestSms(patientAccount, medication, prescription, bloodTest, requestTime, appointmentTime);

                    return result1 & result2;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool SendBloodTestRequestEmail(ApplicationUser patientAccount, Medication medication, Prescription prescription, BloodTest bloodTest, DateTime requestTime, DateTime appointmentTime)
        {
            return _sendEmailService.SendEmail(@$"Hello {patientAccount.FirstName}, you have been requested to have a blood test for your prescription for {medication.MedicationName} starting {prescription.DateStart}. <br>
                                        The test type is: {bloodTest.FullTitle}. <br>
                                        This test was requested at {requestTime} and as scheduled to take place at {appointmentTime}. <br>
                                        Your prescription will not be approved until this test is taken and the results permit the treatment.",
                                        patientAccount.FirstName + " " + patientAccount.LastName + " Blood Test Request",
                                        patientAccount.Email);
        }

        private bool SendBloodTestRequestSms(ApplicationUser patientAccount, Medication medication, Prescription prescription, BloodTest bloodTest, DateTime requestTime, DateTime appointmentTime)
        {
            var textMessage = new StringBuilder()
                .Append($"Hello {patientAccount.FirstName}, you have been requested to have a blood test for your prescription for {medication.MedicationName} starting {prescription.DateStart}.\n\n ")
                .Append($"The test type is: {bloodTest.FullTitle}.")
                .Append($"This test was requested at {requestTime} and as scheduled to take place at {appointmentTime}.")
                .Append($"Your prescription will not be approved until this test is taken and the results permit the treatment.");

            return _sendSmsService.SendSms(patientAccount.PhoneNumber, textMessage.ToString());
        }

        // Blood Test Appointment Time Update #######################################################

        public async Task<bool> SendBloodTestTimeUpdateNotification(Prescription prescription, BloodTestRequest bloodTestRequest, DateTime newTime)
        {
            var patient = GetPatientRecord(prescription.PatientId);
            var medication = GetMedicationRecord(prescription.MedicationId);
            var patientAccount = await _userManager.FindByIdAsync(prescription.PatientId);

            switch (patient.CommunicationPreferences)
            {
                case CommunicationPreferences.Email:
                    return SendBloodTestTimeUpdateEmail(patientAccount, medication, bloodTestRequest, newTime);
                case CommunicationPreferences.Sms:
                    return SendBloodTestTimeUpdateSms(patientAccount, medication, bloodTestRequest, newTime);
                case CommunicationPreferences.Both:
                    var result1 = SendBloodTestTimeUpdateEmail(patientAccount, medication, bloodTestRequest, newTime);
                    var result2 = SendBloodTestTimeUpdateSms(patientAccount, medication, bloodTestRequest, newTime);

                    return result1 & result2;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool SendBloodTestTimeUpdateEmail(ApplicationUser patientAccount, Medication medication, BloodTestRequest bloodTestRequest, DateTime newTime)
        {
            return _sendEmailService.SendEmail(@$"Hello {patientAccount.FirstName}, the blood test appointment time for your {medication.MedicationName} prescription has been updated. <br>
                                        The old time was: {bloodTestRequest.AppointmentTime}, the updated time is: {newTime}. <br>
                                        Your prescription will not be approved until this test is taken and the results permit the treatment.",
                                        patientAccount.FirstName + " " + patientAccount.LastName + " Blood Test Request",
                                        patientAccount.Email);
        }

        private bool SendBloodTestTimeUpdateSms(ApplicationUser patientAccount, Medication medication, BloodTestRequest bloodTestRequest, DateTime newTime)
        {
            var textMessage = new StringBuilder()
                .Append($"Hello {patientAccount.FirstName}, the blood test appointment time for your {medication.MedicationName} prescription has been updated.\n\n ")
                .Append($"The old time was: {bloodTestRequest.AppointmentTime}, the updated time is: {newTime}.")
                .Append($"Your prescription will not be approved until this test is taken and the results permit the treatment.");

            return _sendSmsService.SendSms(patientAccount.PhoneNumber, textMessage.ToString());
        }

        //#######################################################

        private Patient GetPatientRecord(string patientId)
        {
            return _patientService.GetPatients(patientId).ElementAt(0);
        }

        private Medication GetMedicationRecord(int medicationId)
        {
            return _medicationService.GetMedications((short?) medicationId).ElementAt(0);
        }
    }
}