using System;
using System.Collections.Generic;
using System.Linq;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using AADWebApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static AADWebApp.Services.DatabaseService;
using static AADWebApp.Services.PatientService;
using static AADWebApp.Services.PrescriptionCollectionService;
using static AADWebApp.Services.PrescriptionService;

namespace AADWebAppTests.Services
{
    [DoNotParallelize]
    [TestClass]
    public class PrescriptionCollectionServiceTests : TestBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly INotificationService _notificationService;
        private readonly IPrescriptionCollectionService _prescriptionCollectionService;

        public PrescriptionCollectionServiceTests()
        {
            _databaseService = Get<IDatabaseService>();
            _notificationService = Get<INotificationService>();
            _prescriptionCollectionService = new PrescriptionCollectionService(_databaseService, _notificationService);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.ProgramData);

            _databaseService.ExecuteNonQuery($"INSERT INTO Patients (Id, CommunicationPreferences, NhsNumber, GeneralPractitioner) VALUES (1, '{CommunicationPreferences.Email}', 1, 'gp-name');");
            _databaseService.ExecuteNonQuery($"INSERT INTO Prescriptions (MedicationId, PatientId, Dosage, DateStart, DateEnd, PrescriptionStatus, IssueFrequency) VALUES (1, 1, 99, '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', '{PrescriptionStatus.PendingApproval}', 'frequency')");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.ProgramData);

            _databaseService.ExecuteNonQuery($"DELETE FROM PrescriptionCollections;");
            _databaseService.ExecuteNonQuery($"DELETE FROM Prescriptions;");
            _databaseService.ExecuteNonQuery($"DELETE FROM Patients;");

            _databaseService.ExecuteNonQuery($"DBCC CHECKIDENT (Prescriptions, RESEED, 0);");
            _databaseService.ExecuteNonQuery($"DBCC CHECKIDENT (PrescriptionCollections, RESEED, 0);");
        }

        [TestMethod]
        public void WhenThereAreNoPrescriptionCollections()
        {
            var databaseResults = _databaseService.ExecuteScalarQuery("SELECT COUNT(*) FROM PrescriptionCollections");
            var methodResults = _prescriptionCollectionService.GetPrescriptionCollections();

            var enumerable = methodResults.ToList();

            Assert.IsTrue(!enumerable.Any());
            Assert.AreEqual(enumerable.Count(), databaseResults);
        }

        [TestMethod]
        public void WhenGettingPrescriptionCollections()
        {
            var now = DateTime.Now;

            AssertPrescriptionCollectionTableContainsXRows(0);

            // Prep expected
            IEnumerable<PrescriptionCollection> allExpected = new List<PrescriptionCollection>
            {
                new PrescriptionCollection
                {
                    Id = 1,
                    PrescriptionId = 1,
                    CollectionStatus = CollectionStatus.BeingPrepared,
                    CollectionStatusUpdated = now,
                    CollectionTime = now
                },
                new PrescriptionCollection
                {
                    Id = 2,
                    PrescriptionId = 1,
                    CollectionStatus = CollectionStatus.Collected,
                    CollectionStatusUpdated = now,
                    CollectionTime = now
                },
                new PrescriptionCollection
                {
                    Id = 3,
                    PrescriptionId = 1,
                    CollectionStatus = CollectionStatus.CollectionReady,
                    CollectionStatusUpdated = now,
                    CollectionTime = now
                }
            };

            IEnumerable<PrescriptionCollection> singleExpected = allExpected.ToList().Where(p => p.Id == 2);

            var allExpectedSerialised = Serialize(allExpected);
            var singleSerialised = Serialize(singleExpected);

            // Add prescription collections
            var affectedRows1 = _prescriptionCollectionService.CreatePrescriptionCollection(1, CollectionStatus.BeingPrepared, now);
            Assert.AreEqual(1, affectedRows1);

            var affectedRows2 = _prescriptionCollectionService.CreatePrescriptionCollection(1, CollectionStatus.Collected, now);
            Assert.AreEqual(1, affectedRows2);

            var affectedRows3 = _prescriptionCollectionService.CreatePrescriptionCollection(1, CollectionStatus.CollectionReady, now);
            Assert.AreEqual(1, affectedRows3);

            // Check amount of database rows
            var databaseRows = _databaseService.ExecuteScalarQuery("SELECT COUNT(*) FROM PrescriptionCollections");
            Assert.IsTrue(databaseRows == 3);

            // Check results - get prescription collections with no id
            var afterCreateResults = _prescriptionCollectionService.GetPrescriptionCollections();
            var afterCreateResultsSerialised = Serialize(afterCreateResults);

            Assert.IsTrue(afterCreateResults.Count() == 3);
            Assert.AreEqual(allExpectedSerialised, afterCreateResultsSerialised);

            // Check results - get prescription collections with valid id
            var afterCreateResultsByValidId = _prescriptionCollectionService.GetPrescriptionCollections(2);
            var afterCreateResultsByValidIdSerialised = Serialize(afterCreateResultsByValidId);

            Assert.IsTrue(afterCreateResultsByValidId.Count() == 1);
            Assert.AreEqual(singleSerialised, afterCreateResultsByValidIdSerialised);

            // Check results - get prescription collections with invalid id
            var afterCreateResultsByInvalidId = _prescriptionCollectionService.GetPrescriptionCollections(99);
            var afterCreateResultsByInvalidIdSerialised = Serialize(afterCreateResultsByInvalidId);
            var expectedInvalidIdSerialised = Serialize(new List<PrescriptionCollection>());

            Assert.IsTrue(!afterCreateResultsByInvalidId.Any());
            Assert.AreEqual(expectedInvalidIdSerialised, afterCreateResultsByInvalidIdSerialised);
        }

        [TestMethod]
        public void WhenSettingPrescriptionCollectionTimeItIsUpdated()
        {
            var now = DateTime.Now;
            var updatedTime = now.AddDays(1);

            AssertPrescriptionCollectionTableContainsXRows(0);

            // Prep expected
            var originalExpected = AddPrescriptionCollection(1, CollectionStatus.BeingPrepared, now, now);

            IEnumerable<PrescriptionCollection> expectedAfterUpdate = new List<PrescriptionCollection>
            {
                new PrescriptionCollection
                {
                    Id = 1,
                    PrescriptionId = 1,
                    CollectionStatus = CollectionStatus.BeingPrepared,
                    CollectionStatusUpdated = now,
                    CollectionTime = updatedTime
                }
            };

            var originalExpectedSerialised = Serialize(originalExpected);
            var updatedExpectedSerialised = Serialize(expectedAfterUpdate);

            // Make sure they're not the same yet (as we haven't updated)
            Assert.AreNotEqual(originalExpectedSerialised, updatedExpectedSerialised);

            // Update
            var TimeNow = DateTime.Now;
            var TimeTomorrow = DateTime.Now.AddDays(1);

            Prescription prescription = new Prescription
            {
                Id = 1,
                MedicationId = 1,
                PatientId = "1",
                Dosage = 99,
                DateStart = TimeNow,
                DateEnd = TimeTomorrow,
                PrescriptionStatus = PrescriptionStatus.Approved,
                IssueFrequency = IssueFrequency.Monthly
            };

            var affectedRows = _prescriptionCollectionService.SetPrescriptionCollectionTimeAsync(prescription, updatedTime).Result;
            Assert.AreEqual(1, affectedRows);

            // Check there's one database row
            AssertPrescriptionCollectionTableContainsXRows(1);

            // Check results via get prescription collections with id
            var afterUpdateResults = _prescriptionCollectionService.GetPrescriptionCollections(1);

            var expectedUpdatePrescription = expectedAfterUpdate.First(p => p.Id == 1);
            var afterUpdatePrescription = afterUpdateResults.First(p => p.Id == 1);

            Assert.AreEqual(expectedUpdatePrescription.Id, afterUpdatePrescription.Id);
            Assert.AreEqual(expectedUpdatePrescription.PrescriptionId, afterUpdatePrescription.PrescriptionId);
            Assert.AreEqual(expectedUpdatePrescription.CollectionStatus, afterUpdatePrescription.CollectionStatus);
            Assert.IsFalse(expectedUpdatePrescription.CollectionStatusUpdated.Equals(afterUpdatePrescription.CollectionStatusUpdated));
            Assert.IsTrue(expectedUpdatePrescription.CollectionStatusUpdated - afterUpdatePrescription.CollectionStatusUpdated > TimeSpan.Zero);
            Assert.AreEqual(expectedUpdatePrescription.CollectionTime.ToShortDateString(), afterUpdatePrescription.CollectionTime.ToShortDateString());
            Assert.AreEqual(expectedUpdatePrescription.CollectionTime.ToShortTimeString(), afterUpdatePrescription.CollectionTime.ToShortTimeString());
        }

        [TestMethod]
        public void WhenSettingPrescriptionCollectionStatusItIsUpdated()
        {
            var now = DateTime.Now;

            AssertPrescriptionCollectionTableContainsXRows(0);

            // Prep expected
            var originalExpected = AddPrescriptionCollection(1, CollectionStatus.BeingPrepared, now, now);

            IEnumerable<PrescriptionCollection> expectedAfterUpdate = new List<PrescriptionCollection>
            {
                new PrescriptionCollection
                {
                    Id = 1,
                    PrescriptionId = 1,
                    CollectionStatus = CollectionStatus.Collected,
                    CollectionStatusUpdated = now,
                    CollectionTime = now
                }
            };

            var originalExpectedSerialised = Serialize(originalExpected);
            var updatedExpectedSerialised = Serialize(expectedAfterUpdate);

            // Make sure they're not the same yet (as we haven't updated)
            Assert.AreNotEqual(originalExpectedSerialised, updatedExpectedSerialised);

            // Update
            var affectedRows = _prescriptionCollectionService.SetPrescriptionCollectionStatus(1, CollectionStatus.Collected);
            Assert.AreEqual(1, affectedRows);

            // Check there's one database row
            AssertPrescriptionCollectionTableContainsXRows(1);

            // Check results via GetPrescriptionCollections with id
            var afterUpdateResults = _prescriptionCollectionService.GetPrescriptionCollections(1);

            var expectedUpdatePrescription = expectedAfterUpdate.First(p => p.Id == 1);
            var afterUpdatePrescription = afterUpdateResults.First(p => p.Id == 1);

            Assert.AreEqual(expectedUpdatePrescription.Id, afterUpdatePrescription.Id);
            Assert.AreEqual(expectedUpdatePrescription.PrescriptionId, afterUpdatePrescription.PrescriptionId);
            Assert.AreEqual(expectedUpdatePrescription.CollectionStatus, afterUpdatePrescription.CollectionStatus);
            Assert.IsFalse(expectedUpdatePrescription.CollectionStatusUpdated.Equals(afterUpdatePrescription.CollectionStatusUpdated));
            Assert.IsTrue(expectedUpdatePrescription.CollectionStatusUpdated - afterUpdatePrescription.CollectionStatusUpdated > TimeSpan.Zero);
            Assert.AreEqual(expectedUpdatePrescription.CollectionTime.ToShortDateString(), afterUpdatePrescription.CollectionTime.ToShortDateString());
            Assert.AreEqual(expectedUpdatePrescription.CollectionTime.ToShortTimeString(), afterUpdatePrescription.CollectionTime.ToShortTimeString());
        }

        [TestMethod]
        public void WhenCancellingPrescriptionCollectionItIsUpdated()
        {
            var now = DateTime.Now;

            AssertPrescriptionCollectionTableContainsXRows(0);

            // Prep expected
            var originalExpected = AddPrescriptionCollection(1, CollectionStatus.BeingPrepared, now, now);

            IEnumerable<PrescriptionCollection> expectedAfterUpdate = new List<PrescriptionCollection>
            {
                new PrescriptionCollection
                {
                    Id = 1,
                    PrescriptionId = 1,
                    CollectionStatus = CollectionStatus.Cancelled,
                    CollectionStatusUpdated = now,
                    CollectionTime = now
                }
            };

            var originalExpectedSerialised = Serialize(originalExpected);
            var updatedExpectedSerialised = Serialize(expectedAfterUpdate);

            // Make sure they're not the same yet (as we haven't updated)
            Assert.AreNotEqual(originalExpectedSerialised, updatedExpectedSerialised);

            // Update
            var affectedRows = _prescriptionCollectionService.CancelPrescriptionCollection(1);
            Assert.AreEqual(1, affectedRows);

            // Check there's one database row
            AssertPrescriptionCollectionTableContainsXRows(1);

            // Check results via GetPrescriptionCollections with id
            var afterUpdateResults = _prescriptionCollectionService.GetPrescriptionCollections(1);

            var expectedUpdatePrescription = expectedAfterUpdate.First(p => p.Id == 1);
            var afterUpdatePrescription = afterUpdateResults.First(p => p.Id == 1);

            Assert.AreEqual(expectedUpdatePrescription.Id, afterUpdatePrescription.Id);
            Assert.AreEqual(expectedUpdatePrescription.PrescriptionId, afterUpdatePrescription.PrescriptionId);
            Assert.AreEqual(expectedUpdatePrescription.CollectionStatus, afterUpdatePrescription.CollectionStatus);
            Assert.IsFalse(expectedUpdatePrescription.CollectionStatusUpdated.Equals(afterUpdatePrescription.CollectionStatusUpdated));
            Assert.IsTrue(expectedUpdatePrescription.CollectionStatusUpdated - afterUpdatePrescription.CollectionStatusUpdated > TimeSpan.Zero);
            Assert.AreEqual(expectedUpdatePrescription.CollectionTime.ToShortDateString(), afterUpdatePrescription.CollectionTime.ToShortDateString());
            Assert.AreEqual(expectedUpdatePrescription.CollectionTime.ToShortTimeString(), afterUpdatePrescription.CollectionTime.ToShortTimeString());
        }

        private IEnumerable<PrescriptionCollection> AddPrescriptionCollection(int prescriptionId, CollectionStatus collectionStatus, DateTime collectionStatusUpdated, DateTime collectionTime)
        {
            IEnumerable<PrescriptionCollection> expected = new List<PrescriptionCollection>
            {
                new PrescriptionCollection
                {
                    Id = 1,
                    PrescriptionId = prescriptionId,
                    CollectionStatus = collectionStatus,
                    CollectionStatusUpdated = collectionStatusUpdated,
                    CollectionTime = collectionTime
                }
            };

            // Add prescription collection and verify
            var affectedRows1 = _prescriptionCollectionService.CreatePrescriptionCollection(prescriptionId, collectionStatus, collectionTime);
            Assert.AreEqual(1, affectedRows1);

            // Check amount of database rows
            AssertPrescriptionCollectionTableContainsXRows(1);

            return expected;
        }

        private void AssertPrescriptionCollectionTableContainsXRows(int expectedRows)
        {
            // Check prior to make sure there are no prescription_collections
            var databaseResults = _databaseService.ExecuteScalarQuery("SELECT COUNT(*) FROM PrescriptionCollections");
            var methodResults = _prescriptionCollectionService.GetPrescriptionCollections();

            Assert.AreEqual(methodResults.Count(), databaseResults);
            Assert.AreEqual(expectedRows, databaseResults);
        }
    }
}