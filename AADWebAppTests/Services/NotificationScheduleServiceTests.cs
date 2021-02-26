using System;
using System.Linq;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static AADWebApp.Services.PrescriptionService;

namespace AADWebAppTests.Services
{
    [DoNotParallelize]
    [TestClass]
    public class NotificationScheduleServiceTests : TestBase
    {
        private readonly INotificationScheduleService _notificationScheduleService;

        public NotificationScheduleServiceTests()
        {
            _notificationScheduleService = Get<INotificationScheduleService>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            // None required
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var result = _notificationScheduleService.GetPrescriptionSchedules();

            if (result.Count() > 0)
            {
                foreach (var item in result.ToList())
                {
                    _notificationScheduleService.CancelPrescriptionSchedule(item.Prescription.Id);
                }
            }
        }

        [TestMethod]
        public void WhenGettingPrescriptionSchedules()
        {
            AssertPrescriptionScheduleListContainsXSchedules(0);

            var TimeNow = DateTime.Now;
            var TimeTomorrow = DateTime.Now.AddDays(1);

            CreatePrescriptionSchedule(1, 1, "patientId", 77, TimeNow, TimeTomorrow, PrescriptionStatus.PendingApproval);

            Prescription prescription = new Prescription
            {
                Id = 1,
                MedicationId = 1,
                PatientId = "patientId",
                Dosage = 77,
                DateStart = TimeNow,
                DateEnd = TimeTomorrow,
                PrescriptionStatus = PrescriptionStatus.PendingApproval
            };

            var result = _notificationScheduleService.GetPrescriptionSchedules();
            var resultAsList = result.ToList();

            Assert.AreEqual(resultAsList.Count(), 1);

            Assert.AreEqual(prescription.Id, resultAsList[0].Prescription.Id);
            Assert.AreEqual(prescription.MedicationId, resultAsList[0].Prescription.MedicationId);
            Assert.AreEqual(prescription.PatientId, resultAsList[0].Prescription.PatientId);
            Assert.AreEqual(prescription.Dosage, resultAsList[0].Prescription.Dosage);
            Assert.AreEqual(prescription.DateStart, resultAsList[0].Prescription.DateStart);
            Assert.AreEqual(prescription.DateEnd, resultAsList[0].Prescription.DateEnd);
            Assert.AreEqual(prescription.PrescriptionStatus, resultAsList[0].Prescription.PrescriptionStatus);
        }

        [TestMethod]
        public void WhenCreatingPrescriptionSchedules()
        {
            AssertPrescriptionScheduleListContainsXSchedules(0);

            var result = CreatePrescriptionSchedule(1, 1, "patientId", 77, DateTime.Now, DateTime.Now.AddDays(1), PrescriptionStatus.PendingApproval);

            Assert.IsTrue(result);

            AssertPrescriptionScheduleListContainsXSchedules(1);
        }

        [TestMethod]
        public void WhenCancellingPrescriptionSchedules()
        {
            AssertPrescriptionScheduleListContainsXSchedules(0);

            CreatePrescriptionSchedule(1, 1, "patientId", 77, DateTime.Now, DateTime.Now.AddDays(1), PrescriptionStatus.PendingApproval);

            AssertPrescriptionScheduleListContainsXSchedules(1);

            var result = _notificationScheduleService.CancelPrescriptionSchedule(1);

            Assert.IsTrue(result);

            AssertPrescriptionScheduleListContainsXSchedules(0);
        }

        private bool CreatePrescriptionSchedule(int Id, int MedicationId, string PatientId, int Dosage, DateTime TimeNow, DateTime TimeTomorrow, PrescriptionStatus PrescriptionStatus)
        {
            Prescription prescription = new Prescription
            {
                Id = Id,
                MedicationId = MedicationId,
                PatientId = PatientId,
                Dosage = Dosage,
                DateStart = TimeNow,
                DateEnd = TimeTomorrow,
                PrescriptionStatus = PrescriptionStatus
            };

            var result = _notificationScheduleService.CreatePrescriptionSchedule(prescription);

            return result;
        }

        private void AssertPrescriptionScheduleListContainsXSchedules(int expectedCount)
        {
            // Check prior to make sure there are no PrescriptionSchedules
            var count = _notificationScheduleService.GetPrescriptionSchedules().Count();

            Assert.AreEqual(count, expectedCount);
        }
    }
}