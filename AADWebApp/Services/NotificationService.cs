using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AADWebApp.Areas.Identity.Data;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using Microsoft.AspNetCore.Identity;

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

        public async Task SendPrescriptionNotification(Prescription prescription, int occurances)
        {
            var patient = GetPatientRecord(prescription.PatientId);
            var medication = GetMediciationRecord(prescription.MedicationId);
            var patientAccount = await _userManager.FindByIdAsync(prescription.PatientId);

            switch (patient.CommunicationPreferences.ToString())
            {
                case "Email":
                    SendPrescriptionEmail(patientAccount, medication, prescription, occurances);
                    break;
                case "SMS":
                    SendPrescriptionSms(patientAccount, medication, prescription, occurances);
                    break;
                default:
                    SendPrescriptionEmail(patientAccount, medication, prescription, occurances);
                    SendPrescriptionSms(patientAccount, medication, prescription, occurances);
                    break;
            }
        }

        private void SendPrescriptionEmail(ApplicationUser patientAccount, Medication medication, Prescription prescription, int occurances)
        {
            _sendEmailService.SendEmail(@$"Hello {patientAccount.FirstName}, your prescription for {medication.MedicationName} has begun. <br>
                                        You will receive {occurances} dosages of this medication beginning {prescription.DateStart.ToShortDateString()} and ending {prescription.DateEnd.ToShortDateString()}. <br>
                                        The dosage will be {prescription.Dosage} milligrams {prescription.IssueFrequency}.",
                                        patientAccount.FirstName + " " + patientAccount.LastName + " " + medication.MedicationName + " Prescription",
                                        patientAccount.Email);
        }

        private void SendPrescriptionSms(ApplicationUser patientAccount, Medication medication, Prescription prescription, int occurances)
        {
            var textMessage = new StringBuilder()
                .Append($"Hello {patientAccount.FirstName}, your prescription for {medication.MedicationName} has begun.\n\n ")
                .Append($"You will receive {occurances} dosages of this medication beginning {prescription.DateStart.ToShortDateString()}and ending {prescription.DateEnd.ToShortDateString()}.\n\n")
                .Append($"The dosage will be {prescription.Dosage} milligrams {prescription.IssueFrequency}.");

            _sendSmsService.SendSms(patientAccount.PhoneNumber, textMessage.ToString());
        }

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
            _sendEmailService.SendEmail(@$"Hello {patientAccount.FirstName}, you have successfully updated the collection time for your {medication.MedicationName} prescription. <br>
                                        The new date/time is {collectionTime}. <br>
                                        It's current status is {prescription.PrescriptionStatus}.",
                                        patientAccount.FirstName + " " + patientAccount.LastName + " " + medication.MedicationName + " Prescription Collection",
                                        patientAccount.Email);
        }

        private void SendCollectionSms(ApplicationUser patientAccount, Medication medication, Prescription prescription, DateTime collectionTime)
        {
            var textMessage = new StringBuilder()
                .Append($"Hello {patientAccount.FirstName}, you have successfully updated the collection time for your {medication.MedicationName} prescription.\n\n ")
                .Append($"The new date/time is {collectionTime}.\n\n")
                .Append($"It's current status is {prescription.PrescriptionStatus}.");

            _sendSmsService.SendSms(patientAccount.PhoneNumber, textMessage.ToString());
        }

        private Patient GetPatientRecord(string patientId)
        {
            return _patientService.GetPatients(patientId).ElementAt(0);
        }

        private Medication GetMediciationRecord(int medicationId)
        {
            return _medicationService.GetMedications((short?)medicationId).ElementAt(0);
        }
    }
}