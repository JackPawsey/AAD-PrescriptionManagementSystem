using System;
using System.Collections.Generic;
using System.Linq;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using AADWebApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static AADWebApp.Services.DatabaseService;
using static AADWebApp.Services.PrescriptionService;

namespace AADWebAppTests.Services
{
    [DoNotParallelize]
    [TestClass]
    public class PrescriptionServiceTests : TestBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionServiceTests()
        {
            _databaseService = Get<IDatabaseService>();
            _prescriptionService = new PrescriptionService(_databaseService);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.program_data);

            _databaseService.ExecuteNonQuery($"INSERT INTO patients (id, comm_preferences, nhs_number, general_practitioner) VALUES (1, 1, 1, 'gp-name');");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.program_data);

            _databaseService.ExecuteNonQuery($"DELETE FROM prescriptions;");
            _databaseService.ExecuteNonQuery($"DELETE FROM patients;");

            _databaseService.ExecuteNonQuery($"DBCC CHECKIDENT (prescriptions, RESEED, 0);");
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
            var now = DateTime.Now;

            AssertPrescriptionsTableContainsXRows(0);

            // Prep expected
            IEnumerable<Prescription> allExpected = new List<Prescription>
            {
                new Prescription
                {
                    Id = 1,
                    MedicationId = 1,
                    PatientId = "1",
                    Dosage = 77,
                    DateStart = now,
                    DateEnd = now,
                    PrescriptionStatus = PrescriptionStatus.Approved,
                    IssueFrequency = "1frequency"
                },
                new Prescription
                {
                    Id = 2,
                    MedicationId = 2,
                    PatientId = "1",
                    Dosage = 88,
                    DateStart = now,
                    DateEnd = now,
                    PrescriptionStatus = PrescriptionStatus.Declined,
                    IssueFrequency = "2frequency"
                },
                new Prescription
                {
                    Id = 3,
                    MedicationId = 3,
                    PatientId = "1",
                    Dosage = 99,
                    DateStart = now,
                    DateEnd = now,
                    PrescriptionStatus = PrescriptionStatus.PendingApproval,
                    IssueFrequency = "3frequency"
                }
            };

            IEnumerable<Prescription> singleExpected = allExpected.ToList().Where(p => p.Id == 2);

            var allExpectedSerialised = JsonConvert.SerializeObject(allExpected, new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-ddTHH:mm:ss"
            });
            var singleSerialised = JsonConvert.SerializeObject(singleExpected, new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-ddTHH:mm:ss"
            });

            // Add prescriptions
            var affectedRows1 = _prescriptionService.CreatePrescription(1, "1", 77, now, now, PrescriptionStatus.Approved, "1frequency");
            Assert.AreEqual(1, affectedRows1);

            var affectedRows2 = _prescriptionService.CreatePrescription(2, "1", 88, now, now, PrescriptionStatus.Declined, "2frequency");
            Assert.AreEqual(1, affectedRows2);

            var affectedRows3 = _prescriptionService.CreatePrescription(3, "1", 99, now, now, PrescriptionStatus.PendingApproval, "3frequency");
            Assert.AreEqual(1, affectedRows3);

            // Check amount of database rows
            var databaseRows = _databaseService.ExecuteScalarQuery("SELECT COUNT(*) FROM prescriptions");
            Assert.IsTrue(databaseRows == 3);

            // Check results - GetPrescriptions with no id
            var afterCreateResults = _prescriptionService.GetPrescriptions();
            var afterCreateResultsSerialised = JsonConvert.SerializeObject(afterCreateResults);

            Assert.IsTrue(afterCreateResults.Count() == 3);
            Assert.AreEqual(allExpectedSerialised, afterCreateResultsSerialised);

            // Check results - GetPrescriptions with valid id
            var afterCreateResultsByValidId = _prescriptionService.GetPrescriptions(2);
            var afterCreateResultsByValidIdSerialised = JsonConvert.SerializeObject(afterCreateResultsByValidId);

            Assert.IsTrue(afterCreateResultsByValidId.Count() == 1);
            Assert.AreEqual(singleSerialised, afterCreateResultsByValidIdSerialised);

            // Check results - GetPrescriptions with invalid id
            var afterCreateResultsByInvalidId = _prescriptionService.GetPrescriptions(99);
            var afterCreateResultsByInvalidIdSerialised = JsonConvert.SerializeObject(afterCreateResultsByInvalidId);
            var expectedInvalidIdSerialised = JsonConvert.SerializeObject(new List<Patient>());

            Assert.IsTrue(!afterCreateResultsByInvalidId.Any());
            Assert.AreEqual(expectedInvalidIdSerialised, afterCreateResultsByInvalidIdSerialised);
        }

        [TestMethod]
        public void WhenSettingPrescriptionStatusItIsUpdated()
        {
            var now = DateTime.Now;

            AssertPrescriptionsTableContainsXRows(0);

            // Prep expected
            var originalExpected = AddPrescription(1, "1", 99, now, now, PrescriptionStatus.PendingApproval, "frequency");

            IEnumerable<Prescription> expectedAfterUpdate = new List<Prescription>
            {
                new Prescription
                {
                    Id = 1,
                    MedicationId = 1,
                    PatientId = "1",
                    Dosage = 99,
                    DateStart = now,
                    DateEnd = now,
                    PrescriptionStatus = PrescriptionStatus.Approved,
                    IssueFrequency = "frequency"
                }
            };

            var originalExpectedSerialised = JsonConvert.SerializeObject(originalExpected, new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-ddTHH:mm:ss"
            });
            var updatedExpectedSerialised = JsonConvert.SerializeObject(expectedAfterUpdate, new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-ddTHH:mm:ss"
            });

            // Make sure they're not the same yet (as we haven't updated)
            Assert.AreNotEqual(originalExpectedSerialised, updatedExpectedSerialised);

            // Update
            var affectedRows = _prescriptionService.SetPrescriptionStatus(1, PrescriptionStatus.Approved);
            Assert.AreEqual(1, affectedRows);

            // Check there's one database row
            AssertPrescriptionsTableContainsXRows(1);

            // Check results via GetPatients with id
            var afterUpdateResults = _prescriptionService.GetPrescriptions(1);
            var afterUpdateResultsSerialised = JsonConvert.SerializeObject(afterUpdateResults);

            Assert.AreEqual(updatedExpectedSerialised, afterUpdateResultsSerialised);
        }

        [TestMethod]
        public void WhenCancellingPrescriptionItIsUpdated()
        {
            var now = DateTime.Now;

            AssertPrescriptionsTableContainsXRows(0);

            // Prep expected
            var originalExpected = AddPrescription(1, "1", 99, now, now, PrescriptionStatus.PendingApproval, "frequency");

            IEnumerable<Prescription> expectedAfterUpdate = new List<Prescription>
            {
                new Prescription
                {
                    Id = 1,
                    MedicationId = 1,
                    PatientId = "1",
                    Dosage = 99,
                    DateStart = now,
                    DateEnd = now,
                    PrescriptionStatus = PrescriptionStatus.Terminated,
                    IssueFrequency = "frequency"
                }
            };

            var originalExpectedSerialised = JsonConvert.SerializeObject(originalExpected, new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-ddTHH:mm:ss"
            });
            var updatedExpectedSerialised = JsonConvert.SerializeObject(expectedAfterUpdate, new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-ddTHH:mm:ss"
            });

            // Make sure they're not the same yet (as we haven't updated)
            Assert.AreNotEqual(originalExpectedSerialised, updatedExpectedSerialised);

            // Update
            var affectedRows = _prescriptionService.CancelPrescription(1);
            Assert.AreEqual(1, affectedRows);

            // Check there's one database row
            AssertPrescriptionsTableContainsXRows(1);

            // Check results via GetPatients with id
            var afterUpdateResults = _prescriptionService.GetPrescriptions(1);
            var afterUpdateResultsSerialised = JsonConvert.SerializeObject(afterUpdateResults);

            Assert.AreEqual(updatedExpectedSerialised, afterUpdateResultsSerialised);
        }

        private IEnumerable<Prescription> AddPrescription(int MedicationId, string PatientId, int Dosage, DateTime DateStart, DateTime DateEnd, PrescriptionStatus PrescriptionStatus, string IssueFrequency)
        {
            IEnumerable<Prescription> expected = new List<Prescription>
            {
                new Prescription
                {
                    Id = 1,
                    MedicationId = MedicationId,
                    PatientId = PatientId,
                    Dosage = Dosage,
                    DateStart = DateStart,
                    DateEnd = DateEnd,
                    PrescriptionStatus = PrescriptionStatus,
                    IssueFrequency = IssueFrequency
                }
            };

            // Add prescription and verify
            var affectedRows1 = _prescriptionService.CreatePrescription(MedicationId, PatientId, Dosage, DateStart, DateEnd, PrescriptionStatus, IssueFrequency);
            Assert.AreEqual(1, affectedRows1);

            // Check amount of database rows
            AssertPrescriptionsTableContainsXRows(1);

            return expected;
        }

        private void AssertPrescriptionsTableContainsXRows(int expectedRows)
        {
            // Check prior to make sure there are no prescriptions
            var databaseResults = _databaseService.ExecuteScalarQuery("SELECT COUNT(*) FROM prescriptions");
            var methodResults = _prescriptionService.GetPrescriptions();

            Assert.AreEqual(methodResults.Count(), databaseResults);
            Assert.AreEqual(expectedRows, databaseResults);
        }
    }
}
