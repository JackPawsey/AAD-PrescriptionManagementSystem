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
            var patient = _patientService.GetPatients(prescription.PatientId);
            var patientAccount = await _userManager.FindByIdAsync(prescription.PatientId);
            var medication = _medicationService.GetMedications((short?) prescription.MedicationId);

            var enumerable = patient.ToList();
            var medications = medication.ToList();

            switch (enumerable.ElementAt(0).CommunicationPreferences.ToString())
            {
                case "Email":
                    SendEmail(enumerable.ElementAt(0), patientAccount, medications.ElementAt(0), prescription, occurances);
                    break;
                case "SMS":
                    SendSms(enumerable.ElementAt(0), patientAccount, medications.ElementAt(0), prescription, occurances);
                    break;
                default:
                    SendEmail(enumerable.ElementAt(0), patientAccount, medications.ElementAt(0), prescription, occurances);
                    SendSms(enumerable.ElementAt(0), patientAccount, medications.ElementAt(0), prescription, occurances);
                    break;
            }
        }

        private void SendEmail(Patient patient, ApplicationUser patientAccount, Medication medication, Prescription prescription, int occurances)
        {
            _sendEmailService.SendEmail(@$"Hello {patientAccount.FirstName}, your prescription for {medication.MedicationName} has begun. <br>
                                            You will receive {occurances} dosages of this medication beginning {prescription.DateStart.ToShortDateString()} and ending {prescription.DateEnd.ToShortDateString()}. <br>
                                            The dosage will be {prescription.Dosage} milligrams {prescription.IssueFrequency}.",
                patientAccount.FirstName + " " + patientAccount.LastName + " " + medication.MedicationName + " Prescription",
                patientAccount.Email);
        }

        private void SendSms(Patient patient, ApplicationUser patientAccount, Medication medication, Prescription prescription, int occurances)
        {
            var textMessage = new StringBuilder()
                .Append($"Hello {patientAccount.FirstName}, your prescription for {medication.MedicationName} has begun.\n\n ")
                .Append($"You will receive {occurances} dosages of this medication beginning {prescription.DateStart.ToShortDateString()}and ending {prescription.DateEnd.ToShortDateString()}.\n\n")
                .Append($"The dosage will be {prescription.Dosage} milligrams {prescription.IssueFrequency}.");

            _sendSmsService.SendSms(patientAccount.PhoneNumber, textMessage.ToString());
        }
    }
}