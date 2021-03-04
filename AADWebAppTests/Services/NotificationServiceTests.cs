using AADWebApp.Areas.Identity.Data;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using AADWebApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static AADWebApp.Services.DatabaseService;
using static AADWebApp.Services.PatientService;
using static AADWebApp.Services.PrescriptionService;

namespace AADWebAppTests.Services
{
    [DoNotParallelize]
    [TestClass]
    public class NotificationServiceTests : TestBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IPatientService _patientService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISendEmailService _sendEmailService;
        private readonly ISendSmsService _sendSmsService;
        private readonly IMedicationService _medicationService;
        private readonly INotificationService _notificationService;

        public NotificationServiceTests()
        {
            _databaseService = Get<IDatabaseService>();
            _patientService = Get<IPatientService>();
            _sendEmailService = Get<ISendEmailService>();
            _sendSmsService = Get<ISendSmsService>();
            _userManager = Get<UserManager<ApplicationUser>>();
            _medicationService = Get<IMedicationService>();

            _notificationService = new NotificationService(_patientService, _userManager, _sendEmailService, _sendSmsService, _medicationService);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.ProgramData);

            // Tests do pass with email and sns, but will hit monthly sns cost limit very quick so no point
            _databaseService.ExecuteNonQuery($"INSERT INTO Patients (Id, CommunicationPreferences, NhsNumber, GeneralPractitioner) VALUES ('patientId', '{CommunicationPreferences.Email}', 'nhs-number', 'gp-name');");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.ProgramData);

            _databaseService.ExecuteNonQuery($"DELETE FROM Patients;");
        }

        [TestMethod]
        public void WhenSendingPrescriptionNotification()
        {
            var timeNow = DateTime.Now;
            var timeTomorrow = DateTime.Now.AddDays(1);

            var prescripton = new Prescription
            {
                Id = 1,
                MedicationId = 1,
                PatientId = "patientId",
                Dosage = 77,
                DateStart = timeNow,
                DateEnd = timeTomorrow,
                PrescriptionStatus = PrescriptionStatus.AwaitingBloodWork,
                IssueFrequency = IssueFrequency.Weekly
            };

            var result = _notificationService.SendPrescriptionNotification(prescripton, 12, DateTime.Now.AddDays(5)).Result;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WhenSendingCollectionTimeNotification()
        {
            var timeNow = DateTime.Now;
            var timeTomorrow = DateTime.Now.AddDays(1);

            var prescripton = new Prescription
            {
                Id = 1,
                MedicationId = 1,
                PatientId = "patientId",
                Dosage = 77,
                DateStart = timeNow,
                DateEnd = timeTomorrow,
                PrescriptionStatus = PrescriptionStatus.AwaitingBloodWork,
                IssueFrequency = IssueFrequency.Weekly
            };

            var result = _notificationService.SendCollectionTimeNotification(prescripton, DateTime.Now.AddDays(5)).Result;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WhenSendingCancellationNotification()
        {
            var timeNow = DateTime.Now;
            var timeTomorrow = DateTime.Now.AddDays(1);

            var prescripton = new Prescription
            {
                Id = 1,
                MedicationId = 1,
                PatientId = "patientId",
                Dosage = 77,
                DateStart = timeNow,
                DateEnd = timeTomorrow,
                PrescriptionStatus = PrescriptionStatus.AwaitingBloodWork,
                IssueFrequency = IssueFrequency.Weekly
            };

            var result = _notificationService.SendCancellationNotification(prescripton, DateTime.Now).Result;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WhenSendingBloodTestRequestNotification()
        {
            var timeNow = DateTime.Now;
            var timeTomorrow = DateTime.Now.AddDays(1);

            var prescripton = new Prescription
            {
                Id = 1,
                MedicationId = 1,
                PatientId = "patientId",
                Dosage = 77,
                DateStart = timeNow,
                DateEnd = timeTomorrow,
                PrescriptionStatus = PrescriptionStatus.AwaitingBloodWork,
                IssueFrequency = IssueFrequency.Weekly
            };

            var bloodTest = new BloodTest
            {
                Id = 7,
                AbbreviatedTitle = "BP",
                FullTitle = "Blood Pressure",
                RestrictionLevel = 1
            };

            var result = _notificationService.SendBloodTestRequestNotification(prescripton, bloodTest, DateTime.Now).Result;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WhenSendingBloodTestTimeUpdateNotification()
        {
            var timeNow = DateTime.Now;
            var timeTomorrow = DateTime.Now.AddDays(1);

            var prescripton = new Prescription
            {
                Id = 1,
                MedicationId = 1,
                PatientId = "patientId",
                Dosage = 77,
                DateStart = timeNow,
                DateEnd = timeTomorrow,
                PrescriptionStatus = PrescriptionStatus.AwaitingBloodWork,
                IssueFrequency = IssueFrequency.Weekly
            };

            var bloodTestRequest = new BloodTestRequest
            {
                Id = 1,
                PrescriptionId = 1,
                BloodTestId = 7,
                AppointmentTime = timeNow,
                BloodTestStatus = BloodTestService.BloodTestRequestStatus.Pending
            };

            var result = _notificationService.SendBloodTestTimeUpdateNotification(prescripton, bloodTestRequest, DateTime.Now.AddDays(5)).Result;
            Assert.IsTrue(result);
        }
    }
}