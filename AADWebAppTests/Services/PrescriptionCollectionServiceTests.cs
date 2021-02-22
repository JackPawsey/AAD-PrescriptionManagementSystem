﻿using System;
using System.Collections.Generic;
using System.Linq;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using AADWebApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static AADWebApp.Services.DatabaseService;
using static AADWebApp.Services.PrescriptionCollectionService;
using static AADWebApp.Services.PrescriptionService;

namespace AADWebAppTests.Services
{
    [DoNotParallelize]
    [TestClass]
    public class PrescriptionCollectionServiceTests : TestBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IPrescriptionCollectionService _prescriptionCollectionService;

        public PrescriptionCollectionServiceTests()
        {
            _databaseService = Get<IDatabaseService>();
            _prescriptionCollectionService = new PrescriptionCollectionService(_databaseService);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.program_data);

            _databaseService.ExecuteNonQuery($"INSERT INTO patients (id, comm_preferences, nhs_number, general_practitioner) VALUES (1, 1, 1, 'gp-name');");
            _databaseService.ExecuteNonQuery($"INSERT INTO prescriptions (medication_id, patient_id, dosage, date_start, date_end, prescription_status, issue_frequency) VALUES (1, 1, 99, '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', '{PrescriptionStatus.PendingApproval}', 'frequency')");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.program_data);

            _databaseService.ExecuteNonQuery($"DELETE FROM prescription_collections;");
            _databaseService.ExecuteNonQuery($"DELETE FROM prescriptions;");
            _databaseService.ExecuteNonQuery($"DELETE FROM patients;");

            _databaseService.ExecuteNonQuery($"DBCC CHECKIDENT (prescriptions, RESEED, 0);");
            _databaseService.ExecuteNonQuery($"DBCC CHECKIDENT (prescription_collections, RESEED, 0);");
        }

        [TestMethod]
        public void WhenThereAreNoPrescriptionCollections()
        {
            var databaseResults = _databaseService.ExecuteScalarQuery("SELECT COUNT(*) FROM prescription_collections");
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

            var allExpectedSerialised = JsonConvert.SerializeObject(allExpected, new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-ddTHH:mm:ss"
            });
            var singleSerialised = JsonConvert.SerializeObject(singleExpected, new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-ddTHH:mm:ss"
            });

            // Add prescription collections
            var affectedRows1 = _prescriptionCollectionService.CreatePrescriptionCollection(1, CollectionStatus.BeingPrepared, now, now);
            Assert.AreEqual(1, affectedRows1);

            var affectedRows2 = _prescriptionCollectionService.CreatePrescriptionCollection(1, CollectionStatus.Collected, now, now);
            Assert.AreEqual(1, affectedRows2);

            var affectedRows3 = _prescriptionCollectionService.CreatePrescriptionCollection(1, CollectionStatus.CollectionReady, now, now);
            Assert.AreEqual(1, affectedRows3);

            // Check amount of database rows
            var databaseRows = _databaseService.ExecuteScalarQuery("SELECT COUNT(*) FROM prescription_collections");
            Assert.IsTrue(databaseRows == 3);

            // Check results - get prescription collections with no id
            var afterCreateResults = _prescriptionCollectionService.GetPrescriptionCollections();
            var afterCreateResultsSerialised = JsonConvert.SerializeObject(afterCreateResults);

            Assert.IsTrue(afterCreateResults.Count() == 3);
            Assert.AreEqual(allExpectedSerialised, afterCreateResultsSerialised);

            // Check results - get prescription collections with valid id
            var afterCreateResultsByValidId = _prescriptionCollectionService.GetPrescriptionCollections(2);
            var afterCreateResultsByValidIdSerialised = JsonConvert.SerializeObject(afterCreateResultsByValidId);

            Assert.IsTrue(afterCreateResultsByValidId.Count() == 1);
            Assert.AreEqual(singleSerialised, afterCreateResultsByValidIdSerialised);

            // Check results - get prescription collections with invalid id
            var afterCreateResultsByInvalidId = _prescriptionCollectionService.GetPrescriptionCollections(99);
            var afterCreateResultsByInvalidIdSerialised = JsonConvert.SerializeObject(afterCreateResultsByInvalidId);
            var expectedInvalidIdSerialised = JsonConvert.SerializeObject(new List<PrescriptionCollection>());

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
            var affectedRows = _prescriptionCollectionService.SetPrescriptionCollectionTime(1, updatedTime);
            Assert.AreEqual(1, affectedRows);

            // Check there's one database row
            AssertPrescriptionCollectionTableContainsXRows(1);

            // Check results via get prescription collections with id
            var afterUpdateResults = _prescriptionCollectionService.GetPrescriptionCollections(1);
            var afterUpdateResultsSerialised = JsonConvert.SerializeObject(afterUpdateResults);

            Assert.AreEqual(updatedExpectedSerialised, afterUpdateResultsSerialised);
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
            var affectedRows = _prescriptionCollectionService.SetPrescriptionCollectionStatus(1, CollectionStatus.Collected);
            Assert.AreEqual(1, affectedRows);

            // Check there's one database row
            AssertPrescriptionCollectionTableContainsXRows(1);

            // Check results via GetPrescriptionCollections with id
            var afterUpdateResults = _prescriptionCollectionService.GetPrescriptionCollections(1);

            Assert.AreEqual(expectedAfterUpdate.ElementAt(0).Id, afterUpdateResults.ElementAt(0).Id);
            Assert.AreEqual(expectedAfterUpdate.ElementAt(0).PrescriptionId, afterUpdateResults.ElementAt(0).PrescriptionId);
            Assert.AreEqual(expectedAfterUpdate.ElementAt(0).CollectionStatus, afterUpdateResults.ElementAt(0).CollectionStatus);
            Assert.IsFalse(expectedAfterUpdate.ElementAt(0).CollectionStatusUpdated.Equals(afterUpdateResults.ElementAt(0).CollectionStatusUpdated));
            Assert.AreEqual(expectedAfterUpdate.ElementAt(0).CollectionTime.ToShortDateString(), afterUpdateResults.ElementAt(0).CollectionTime.ToShortDateString());
            Assert.AreEqual(expectedAfterUpdate.ElementAt(0).CollectionTime.ToShortTimeString(), afterUpdateResults.ElementAt(0).CollectionTime.ToShortTimeString());
        }

        private IEnumerable<PrescriptionCollection> AddPrescriptionCollection(int PrescriptionId, CollectionStatus CollectionStatus, DateTime CollectionStatusUpdated, DateTime CollectionTime)
        {
            IEnumerable<PrescriptionCollection> expected = new List<PrescriptionCollection>
            {
                new PrescriptionCollection
                {
                    Id = 1,
                    PrescriptionId = PrescriptionId,
                    CollectionStatus = CollectionStatus,
                    CollectionStatusUpdated = CollectionStatusUpdated,
                    CollectionTime = CollectionTime
                }
            };

            // Add prescription collection and verify
            var affectedRows1 = _prescriptionCollectionService.CreatePrescriptionCollection(PrescriptionId, CollectionStatus, CollectionStatusUpdated, CollectionTime);
            Assert.AreEqual(1, affectedRows1);

            // Check amount of database rows
            AssertPrescriptionCollectionTableContainsXRows(1);

            return expected;
        }

        private void AssertPrescriptionCollectionTableContainsXRows(int expectedRows)
        {
            // Check prior to make sure there are no prescription_collections
            var databaseResults = _databaseService.ExecuteScalarQuery("SELECT COUNT(*) FROM prescription_collections");
            var methodResults = _prescriptionCollectionService.GetPrescriptionCollections();

            Assert.AreEqual(methodResults.Count(), databaseResults);
            Assert.AreEqual(expectedRows, databaseResults);
        }
    }
}
