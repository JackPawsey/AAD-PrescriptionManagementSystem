﻿using System;
using System.Collections.Generic;
using System.Linq;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using AADWebApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static AADWebApp.Services.DatabaseService;
using static AADWebApp.Services.PatientService;
using static AADWebApp.Services.PrescriptionService;

namespace AADWebAppTests.Services
{
    [DoNotParallelize]
    [TestClass]
    public class PrescriptionServiceTests : TestBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly INotificationScheduleService _notificationScheduleService;
        private readonly IPrescriptionService _prescriptionService;
        private readonly INotificationService _notificationService;
        private readonly IBloodTestService _bloodTestService;
        private readonly IPrescriptionCollectionService _prescriptionCollectionService;

        public PrescriptionServiceTests()
        {
            _databaseService = Get<IDatabaseService>();
            _notificationScheduleService = Get<INotificationScheduleService>();
            _notificationService = Get<INotificationService>();
            _bloodTestService = Get<IBloodTestService>();
            _prescriptionCollectionService = Get<IPrescriptionCollectionService>();
            _prescriptionService = new PrescriptionService(_databaseService, _notificationScheduleService, _notificationService, _prescriptionCollectionService, _bloodTestService);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.ProgramData);

            _databaseService.ExecuteNonQuery($"INSERT INTO Patients (Id, CommunicationPreferences, NhsNumber, GeneralPractitioner) VALUES ('patientId', '{CommunicationPreferences.Email}', 'nhs-number', 'gp-name');");
            _databaseService.ExecuteNonQuery($"INSERT INTO Patients (Id, CommunicationPreferences, NhsNumber, GeneralPractitioner) VALUES ('patientId2', '{CommunicationPreferences.Email}', 'nhs-number-2', 'gp-name');");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.ProgramData);

            _databaseService.ExecuteNonQuery($"DELETE FROM Prescriptions;");
            _databaseService.ExecuteNonQuery($"DELETE FROM Patients;");

            _databaseService.ExecuteNonQuery($"DBCC CHECKIDENT (Prescriptions, RESEED, 0);");
        }

        [TestMethod]
        public void WhenThereAreNoPrescriptions()
        {
            var databaseResults = _databaseService.ExecuteScalarQuery("SELECT COUNT(*) FROM prescriptions");
            var methodResults = _prescriptionService.GetPrescriptions();

            var enumerable = methodResults.ToList();

            Assert.IsTrue(!enumerable.Any());
            Assert.AreEqual(enumerable.Count(), databaseResults);
        }

        [TestMethod]
        public void WhenGettingPrescriptions()
        {
            var timeNow = DateTime.Now;
            var timeTomorrow = DateTime.Now.AddDays(1);

            AssertPrescriptionsTableContainsXRows(0);

            // Prep expected
            IEnumerable<Prescription> allExpected = new List<Prescription>
            {
                new Prescription
                {
                    Id = 1,
                    MedicationId = 1,
                    PatientId = "patientId",
                    Dosage = 77,
                    DateStart = timeNow,
                    DateEnd = timeTomorrow,
                    PrescriptionStatus = PrescriptionStatus.PendingApproval,
                    IssueFrequency = IssueFrequency.Weekly
                },
                new Prescription
                {
                    Id = 2,
                    MedicationId = 2,
                    PatientId = "patientId",
                    Dosage = 88,
                    DateStart = timeNow,
                    DateEnd = timeTomorrow,
                    PrescriptionStatus = PrescriptionStatus.PendingApproval,
                    IssueFrequency = IssueFrequency.BiWeekly
                },
                new Prescription
                {
                    Id = 3,
                    MedicationId = 3,
                    PatientId = "patientId",
                    Dosage = 99,
                    DateStart = timeNow,
                    DateEnd = timeTomorrow,
                    PrescriptionStatus = PrescriptionStatus.PendingApproval,
                    IssueFrequency = IssueFrequency.Monthly
                }
            };

            var singleExpected = allExpected.ToList().Where(p => p.Id == 2);

            var allExpectedSerialised = Serialize(allExpected);
            var singleSerialised = Serialize(singleExpected);

            // Add prescriptions
            var affectedRows1 = _prescriptionService.CreatePrescription(1, "patientId", 77, timeNow, timeTomorrow, IssueFrequency.Weekly);
            Assert.AreEqual(1, affectedRows1);

            var affectedRows2 = _prescriptionService.CreatePrescription(2, "patientId", 88, timeNow, timeTomorrow, IssueFrequency.BiWeekly);
            Assert.AreEqual(1, affectedRows2);

            var affectedRows3 = _prescriptionService.CreatePrescription(3, "patientId", 99, timeNow, timeTomorrow, IssueFrequency.Monthly);
            Assert.AreEqual(1, affectedRows3);

            // Check amount of database rows
            AssertPrescriptionsTableContainsXRows(3);

            // Check results - GetPrescriptions with no id
            var afterCreateResults = _prescriptionService.GetPrescriptions();
            var afterCreateResultsSerialised = Serialize(afterCreateResults);

            Assert.IsTrue(afterCreateResults.Count() == 3);
            Assert.AreEqual(allExpectedSerialised, afterCreateResultsSerialised);

            // Check results - GetPrescriptions with valid id
            var afterCreateResultsByValidId = _prescriptionService.GetPrescriptions(2);
            var afterCreateResultsByValidIdSerialised = Serialize(afterCreateResultsByValidId);

            Assert.IsTrue(afterCreateResultsByValidId.Count() == 1);
            Assert.AreEqual(singleSerialised, afterCreateResultsByValidIdSerialised);

            // Check results - GetPrescriptions with invalid id
            var afterCreateResultsByInvalidId = _prescriptionService.GetPrescriptions(99);
            var afterCreateResultsByInvalidIdSerialised = Serialize(afterCreateResultsByInvalidId);
            var expectedInvalidIdSerialised = Serialize(new List<Prescription>());

            Assert.IsTrue(!afterCreateResultsByInvalidId.Any());
            Assert.AreEqual(expectedInvalidIdSerialised, afterCreateResultsByInvalidIdSerialised);
        }

        [TestMethod]
        public void WhenGettingPrescriptionsByPatientId()
        {
            var timeNow = DateTime.Now;
            var timeTomorrow = DateTime.Now.AddDays(1);

            AssertPrescriptionsTableContainsXRows(0);

            // Prep expected
            IEnumerable<Prescription> allExpected = new List<Prescription>
            {
                new Prescription
                {
                    Id = 1,
                    MedicationId = 1,
                    PatientId = "patientId2",
                    Dosage = 77,
                    DateStart = timeNow,
                    DateEnd = timeTomorrow,
                    PrescriptionStatus = PrescriptionStatus.PendingApproval,
                    IssueFrequency = IssueFrequency.Weekly
                },
                new Prescription
                {
                    Id = 2,
                    MedicationId = 2,
                    PatientId = "patientId",
                    Dosage = 88,
                    DateStart = timeNow,
                    DateEnd = timeTomorrow,
                    PrescriptionStatus = PrescriptionStatus.PendingApproval,
                    IssueFrequency = IssueFrequency.BiWeekly
                },
                new Prescription
                {
                    Id = 3,
                    MedicationId = 3,
                    PatientId = "patientId",
                    Dosage = 99,
                    DateStart = timeNow,
                    DateEnd = timeTomorrow,
                    PrescriptionStatus = PrescriptionStatus.PendingApproval,
                    IssueFrequency = IssueFrequency.Monthly
                }
            };

            var allExpectedSerialised = Serialize(allExpected);

            IEnumerable<Prescription> singleExpected = new List<Prescription>
            {
                new Prescription
                {
                    Id = 1,
                    MedicationId = 1,
                    PatientId = "patientId2",
                    Dosage = 77,
                    DateStart = timeNow,
                    DateEnd = timeTomorrow,
                    PrescriptionStatus = PrescriptionStatus.PendingApproval,
                    IssueFrequency = IssueFrequency.Weekly
                }
            };

            var singleExpectedSerialised = Serialize(singleExpected);

            // Add prescriptions
            var affectedRows1 = _prescriptionService.CreatePrescription(1, "patientId2", 77, timeNow, timeTomorrow, IssueFrequency.Weekly);
            Assert.AreEqual(1, affectedRows1);

            var affectedRows2 = _prescriptionService.CreatePrescription(2, "patientId", 88, timeNow, timeTomorrow, IssueFrequency.BiWeekly);
            Assert.AreEqual(1, affectedRows2);

            var affectedRows3 = _prescriptionService.CreatePrescription(3, "patientId", 99, timeNow, timeTomorrow, IssueFrequency.Monthly);
            Assert.AreEqual(1, affectedRows3);

            // Check amount of database rows
            AssertPrescriptionsTableContainsXRows(3);

            // Check results - GetPrescriptions with no id
            var afterCreateResults = _prescriptionService.GetPrescriptionsByPatientId();
            var afterCreateResultsSerialised = Serialize(afterCreateResults);

            Assert.IsTrue(afterCreateResults.Count() == 3);
            Assert.AreEqual(allExpectedSerialised, afterCreateResultsSerialised);

            // Check results - GetPrescriptions with valid id
            var afterCreateResultsByValidId = _prescriptionService.GetPrescriptionsByPatientId("patientId2");
            var afterCreateResultsByValidIdSerialised = Serialize(afterCreateResultsByValidId);

            Assert.IsTrue(afterCreateResultsByValidId.Count() == 1);
            Assert.AreEqual(singleExpectedSerialised, afterCreateResultsByValidIdSerialised);

            // Check results - GetPrescriptions with invalid id
            var afterCreateResultsByInvalidId = _prescriptionService.GetPrescriptionsByPatientId("InvalidPatientId");
            var afterCreateResultsByInvalidIdSerialised = Serialize(afterCreateResultsByInvalidId);
            var expectedInvalidIdSerialised = Serialize(new List<Prescription>());

            Assert.IsTrue(!afterCreateResultsByInvalidId.Any());
            Assert.AreEqual(expectedInvalidIdSerialised, afterCreateResultsByInvalidIdSerialised);
        }

        [TestMethod]
        public void WhenSettingPrescriptionStatusItIsUpdated()
        {
            var timeNow = DateTime.Now;
            var timeTomorrow = DateTime.Now.AddDays(1);

            AssertPrescriptionsTableContainsXRows(0);

            // Prep expected
            var originalExpected = AddPrescription(1, "patientId", 99, timeNow, timeTomorrow, IssueFrequency.Monthly);

            IEnumerable<Prescription> expectedAfterUpdate = new List<Prescription>
            {
                new Prescription
                {
                    Id = 1,
                    MedicationId = 1,
                    PatientId = "patientId",
                    Dosage = 99,
                    DateStart = timeNow,
                    DateEnd = timeTomorrow,
                    PrescriptionStatus = PrescriptionStatus.Approved,
                    IssueFrequency = IssueFrequency.Monthly
                }
            };

            var originalExpectedSerialised = Serialize(originalExpected);
            var updatedExpectedSerialised = Serialize(expectedAfterUpdate);

            // Make sure they're not the same yet (as we haven't updated)
            Assert.AreNotEqual(originalExpectedSerialised, updatedExpectedSerialised);

            // Update
            var affectedRows = _prescriptionService.SetPrescriptionStatus(1, PrescriptionStatus.Approved);
            Assert.AreEqual(1, affectedRows);

            // Check there's one database row
            AssertPrescriptionsTableContainsXRows(1);

            // Check results via GetPatients with id
            var afterUpdateResults = _prescriptionService.GetPrescriptions(1);
            var afterUpdateResultsSerialised = Serialize(afterUpdateResults);

            Assert.AreEqual(updatedExpectedSerialised, afterUpdateResultsSerialised);
        }

        [TestMethod]
        public void WhenCancellingPrescriptionItIsUpdated()
        {
            var timeNow = DateTime.Now;
            var timeTomorrow = DateTime.Now.AddDays(1);

            AssertPrescriptionsTableContainsXRows(0);

            // Prep expected
            var originalExpected = AddPrescription(1, "patientId", 99, timeNow, timeTomorrow, IssueFrequency.Monthly);

            IEnumerable<Prescription> expectedAfterUpdate = new List<Prescription>
            {
                new Prescription
                {
                    Id = 1,
                    MedicationId = 1,
                    PatientId = "patientId",
                    Dosage = 99,
                    DateStart = timeNow,
                    DateEnd = timeTomorrow,
                    PrescriptionStatus = PrescriptionStatus.Terminated,
                    IssueFrequency = IssueFrequency.Monthly
                }
            };

            var originalExpectedSerialised = Serialize(originalExpected);
            var updatedExpectedSerialised = Serialize(expectedAfterUpdate);

            // Make sure they're not the same yet (as we haven't updated)
            Assert.AreNotEqual(originalExpectedSerialised, updatedExpectedSerialised);

            // Update
            var affectedRows = _prescriptionService.CancelPrescriptionAsync(1).Result;
            Assert.AreEqual(1, affectedRows);

            // Check there's one database row
            AssertPrescriptionsTableContainsXRows(1);

            // Check results via GetPatients with id
            var afterUpdateResults = _prescriptionService.GetPrescriptions(1);
            var afterUpdateResultsSerialised = Serialize(afterUpdateResults);

            Assert.AreEqual(updatedExpectedSerialised, afterUpdateResultsSerialised);
        }

        private IEnumerable<Prescription> AddPrescription(short medicationId, string patientId, int dosage, DateTime dateStart, DateTime dateEnd, IssueFrequency issueFrequency)
        {
            IEnumerable<Prescription> expected = new List<Prescription>
            {
                new Prescription
                {
                    Id = 1,
                    MedicationId = medicationId,
                    PatientId = patientId,
                    Dosage = dosage,
                    DateStart = dateStart,
                    DateEnd = dateEnd,
                    PrescriptionStatus = PrescriptionStatus.PendingApproval,
                    IssueFrequency = issueFrequency
                }
            };

            // Add prescription and verify
            var affectedRows1 = _prescriptionService.CreatePrescription(medicationId, patientId, dosage, dateStart, dateEnd, issueFrequency);
            Assert.AreEqual(1, affectedRows1);

            // Check amount of database rows
            AssertPrescriptionsTableContainsXRows(1);

            return expected;
        }

        private void AssertPrescriptionsTableContainsXRows(int expectedRows)
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.ProgramData);
            
            // Check prior to make sure there are no prescriptions
            var databaseResults = _databaseService.ExecuteScalarQuery("SELECT COUNT(*) FROM Prescriptions");
            var methodResults = _prescriptionService.GetPrescriptions();

            Assert.AreEqual(methodResults.Count(), databaseResults);
            Assert.AreEqual(expectedRows, databaseResults);
        }
    }
}