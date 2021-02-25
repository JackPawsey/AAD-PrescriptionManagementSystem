using AADWebApp.Areas.Identity.Data;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Console.WriteLine("NOTIFICATION SERVICE");
            _patientService = patientService;
            _userManager = userManager;
            _sendEmailService = sendEmailService;
            _sendSmsService = sendSmsService;
            _medicationService = medicationService;
        }
        public async Task SendPrescriptionNotification(Prescription Prescription, int Occurances)
        {
            //Console.WriteLine(Prescription.PatientId);
            var patient = _patientService.GetPatients(Prescription.PatientId);
            var patientAccount = await _userManager.FindByIdAsync(Prescription.PatientId);
            var medication = _medicationService.GetMedications((short?) Prescription.MedicationId);

            //Console.WriteLine("2");

            if (patient.ElementAt(0).CommunicationPreferences.ToString().Equals("Email"))
            {
                _sendEmailService.SendEmail(@$"Hello {patientAccount.FirstName}, your prescription for {medication.ElementAt(0).MedicationName} has begun. <br>
                                            You will receive {Occurances} dosages of this medication",
                                            patientAccount.FirstName + " " + patientAccount.LastName + " " + medication.ElementAt(0).MedicationName + " Prescription", 
                                            patientAccount.Email);
            }
            else if (patient.ElementAt(0).CommunicationPreferences.ToString().Equals("SMS"))
            {
                _sendSmsService.SendSms(patientAccount.PhoneNumber, "Hello " + patientAccount.FirstName + ", your prescription (" + Prescription.Id + ") has begun.");
            }
            else
            {
                _sendEmailService.SendEmail("test", patientAccount.FirstName + " " + patientAccount.LastName + " " + Prescription.Id, patientAccount.Email);
                _sendSmsService.SendSms(patientAccount.PhoneNumber, "Hello " + patientAccount.FirstName + ", your prescription (" + Prescription.Id + ") has begun.");
            }
            //Console.WriteLine("3");
        }
    }
}
