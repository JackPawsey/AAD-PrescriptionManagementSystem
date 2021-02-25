using AADWebApp.Areas.Identity.Data;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task SendPrescriptionNotification(Prescription Prescription, int Occurances)
        {
            var patient = _patientService.GetPatients(Prescription.PatientId);
            var patientAccount = await _userManager.FindByIdAsync(Prescription.PatientId);
            var medication = _medicationService.GetMedications((short?)Prescription.MedicationId);

            if (patient.ElementAt(0).CommunicationPreferences.ToString().Equals("Email"))
            {
                SendEmail(patient.ElementAt(0), patientAccount, medication.ElementAt(0), Prescription, Occurances);
            }
            else if (patient.ElementAt(0).CommunicationPreferences.ToString().Equals("SMS"))
            {
                SendSMS(patient.ElementAt(0), patientAccount, medication.ElementAt(0), Prescription, Occurances);
            }
            else
            {
                SendEmail(patient.ElementAt(0), patientAccount, medication.ElementAt(0), Prescription, Occurances);
                SendSMS(patient.ElementAt(0), patientAccount, medication.ElementAt(0), Prescription, Occurances);
            }
        }

        private void SendEmail(Patient patient, ApplicationUser patientAccount, Medication medication, Prescription Prescription, int Occurances)
        {
            _sendEmailService.SendEmail(@$"Hello {patientAccount.FirstName}, your prescription for {medication.MedicationName} has begun. <br>
                                            You will receive {Occurances} dosages of this medication beginning {Prescription.DateStart.ToShortDateString()} and ending {Prescription.DateEnd.ToShortDateString()}. <br>
                                            The dosage will be {Prescription.Dosage} milligrams {Prescription.IssueFrequency}.",
                                            patientAccount.FirstName + " " + patientAccount.LastName + " " + medication.MedicationName + " Prescription",
                                            patientAccount.Email);
        }

        private void SendSMS(Patient patient, ApplicationUser patientAccount, Medication medication, Prescription Prescription, int Occurances)
        {
            var TextMessage = new StringBuilder()
                .Append($"Hello {patientAccount.FirstName}, your prescription for {medication.MedicationName} has begun.\n\n ")
                .Append($"You will receive { Occurances} dosages of this medication beginning { Prescription.DateStart.ToShortDateString()}and ending { Prescription.DateEnd.ToShortDateString()}.\n\n")
                .Append($"The dosage will be { Prescription.Dosage} milligrams { Prescription.IssueFrequency}.");

            _sendSmsService.SendSms(patientAccount.PhoneNumber, TextMessage.ToString());
        }
    }
}
