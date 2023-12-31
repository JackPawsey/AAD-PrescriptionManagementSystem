﻿using System;
using System.Collections.Generic;
using System.Linq;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using AADWebApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static AADWebApp.Services.BloodTestService;
using static AADWebApp.Services.DatabaseService;
using static AADWebApp.Services.PatientService;
using static AADWebApp.Services.PrescriptionService;

namespace AADWebAppTests.Services
{
    [TestClass]
    public class BloodTestServiceTests : TestBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IBloodTestService _bloodTestService;
        private readonly INotificationService _notificationService;

        public BloodTestServiceTests()
        {
            _databaseService = Get<IDatabaseService>();
            _notificationService = Get<INotificationService>();
            _bloodTestService = new BloodTestService(_databaseService, _notificationService);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.ProgramData);

            // Populate a prescription for some tests, which means also creating a user due to the table constraints
            _databaseService.ExecuteNonQuery($"INSERT INTO Patients (Id, CommunicationPreferences, NhsNumber, GeneralPractitioner) VALUES (1, '{CommunicationPreferences.Email}', 1, 'gp-name');");
            _databaseService.ExecuteNonQuery($"INSERT INTO Prescriptions (MedicationId, PatientId, Dosage, DateStart, DateEnd, PrescriptionStatus, IssueFrequency) VALUES (1, 1, 1, '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', 'Approved', 'Weekly');");
            _databaseService.ExecuteNonQuery($"INSERT INTO Prescriptions (MedicationId, PatientId, Dosage, DateStart, DateEnd, PrescriptionStatus, IssueFrequency) VALUES (2, 1, 1, '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', 'Approved', 'Weekly');");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.ProgramData);

            // Clear the tables
            _databaseService.ExecuteNonQuery($"DELETE FROM BloodTestResults;");
            _databaseService.ExecuteNonQuery($"DELETE FROM BloodTestRequests;");
            _databaseService.ExecuteNonQuery($"DELETE FROM Prescriptions;");
            _databaseService.ExecuteNonQuery($"DELETE FROM Patients;");

            // Reset auto increment values for next test
            _databaseService.ExecuteNonQuery($"DBCC CHECKIDENT (BloodTestResults, RESEED, 0);");
            _databaseService.ExecuteNonQuery($"DBCC CHECKIDENT (BloodTestRequests, RESEED, 0);");
            _databaseService.ExecuteNonQuery($"DBCC CHECKIDENT (Prescriptions, RESEED, 0);");
        }

        [TestMethod]
        public void WhenThereAreBloodTests()
        {
            var results = _bloodTestService.GetBloodTests();

            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void WhenGettingBloodTestByAValidId()
        {
            // Prep Expected
            IEnumerable<BloodTest> expected = new List<BloodTest>
            {
                new BloodTest
                {
                    Id = 6,
                    AbbreviatedTitle = "FBC",
                    FullTitle = "Full Blood Count",
                    RestrictionLevel = 1
                }
            };

            var expectedSerialised = Serialize(expected);

            // GetBloodTests with id 6
            var results = _bloodTestService.GetBloodTests(6);

            // Check results
            var resultSerialised = Serialize(results);

            Assert.IsTrue(results.Count() == 1);
            Assert.AreEqual(expectedSerialised, resultSerialised);
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(1000)]
        public void WhenGettingBloodTestByAnInvalidId(int id)
        {
            var results = _bloodTestService.GetBloodTests((short) id); // This case is necessary as DataRow is dumb and only works with primitives

            Assert.IsTrue(!results.Any());
        }

        // GetBloodTestResults test methods
        [TestMethod]
        public void WhenThereAreNoBloodTestResults()
        {
            var results = _bloodTestService.GetBloodTestResults();

            Assert.IsTrue(!results.Any());
        }

        // GetBloodTestRequests test methods
        [TestMethod]
        public void WhenThereAreNoBloodTestRequests()
        {
            var results = _bloodTestService.GetBloodTestRequests();

            Assert.IsTrue(!results.Any());
        }

        // RequestBloodTest test method
        [TestMethod]
        public void WhenRequestingBloodTestsRowsAreAdded()
        {
            // Check prior to any setup - via the database
            AssertBloodTestRequestsTableContainsXRows(0);

            var beforeRequestResults = _bloodTestService.GetBloodTestRequests().ToList();
            Assert.IsTrue(!beforeRequestResults.Any());

            var timeNow = DateTime.Now;
            var timeTomorrow = DateTime.Now.AddDays(1);

            // Prep expected
            IEnumerable<BloodTestRequest> allExpected = new List<BloodTestRequest>
            {
                new BloodTestRequest
                {
                    Id = 1,
                    PrescriptionId = 1,
                    BloodTestId = 1,
                    AppointmentTime = null,
                    BloodTestStatus = BloodTestRequestStatus.Pending
                },
                new BloodTestRequest
                {
                    Id = 2,
                    PrescriptionId = 2,
                    BloodTestId = 3,
                    AppointmentTime = null,
                    BloodTestStatus = BloodTestRequestStatus.Pending
                },
                new BloodTestRequest
                {
                    Id = 3,
                    PrescriptionId = 1,
                    BloodTestId = 1,
                    AppointmentTime = null,
                    BloodTestStatus = BloodTestRequestStatus.Pending
                }
            };

            IEnumerable<BloodTestRequest> expected1 = new List<BloodTestRequest>
            {
                new BloodTestRequest
                {
                    Id = 1,
                    PrescriptionId = 1,
                    BloodTestId = 1,
                    AppointmentTime = null,
                    BloodTestStatus = BloodTestRequestStatus.Pending
                },
                new BloodTestRequest
                {
                    Id = 3,
                    PrescriptionId = 1,
                    BloodTestId = 1,
                    AppointmentTime = null,
                    BloodTestStatus = BloodTestRequestStatus.Pending
                }
            };

            IEnumerable<BloodTestRequest> expected2 = new List<BloodTestRequest>
            {
                new BloodTestRequest
                {
                    Id = 2,
                    PrescriptionId = 2,
                    BloodTestId = 3,
                    AppointmentTime = null,
                    BloodTestStatus = BloodTestRequestStatus.Pending
                }
            };

            var allExpectedSerialised = Serialize(allExpected);

            var expected1Serialised = Serialize(expected1);

            var expected2Serialised = Serialize(expected2);

            var prescription1 = new Prescription
            {
                Id = 1,
                MedicationId = 1,
                PatientId = "1",
                Dosage = 99,
                DateStart = timeNow,
                DateEnd = timeTomorrow,
                PrescriptionStatus = PrescriptionStatus.PendingApproval,
                IssueFrequency = IssueFrequency.Monthly
            };

            var prescription2 = new Prescription
            {
                Id = 2,
                MedicationId = 2,
                PatientId = "1",
                Dosage = 99,
                DateStart = timeNow,
                DateEnd = timeTomorrow,
                PrescriptionStatus = PrescriptionStatus.PendingApproval,
                IssueFrequency = IssueFrequency.Monthly
            };

            // Request blood tests
            var affectedRows1 = _bloodTestService.RequestBloodTestAsync(prescription1, 1).Result;
            Assert.AreEqual(1, affectedRows1);

            var affectedRows2 = _bloodTestService.RequestBloodTestAsync(prescription2, 3).Result;
            Assert.AreEqual(1, affectedRows2);

            var affectedRows3 = _bloodTestService.RequestBloodTestAsync(prescription1, 1).Result;
            Assert.AreEqual(1, affectedRows3);

            // Check amount of database rows
            AssertBloodTestRequestsTableContainsXRows(3);

            // Check results - GetBloodTestRequests with no id
            var afterRequestResults = _bloodTestService.GetBloodTestRequestsByPrescriptionId();
            var afterRequestResultSerialised = Serialize(afterRequestResults);

            Assert.IsTrue(afterRequestResults.Count() == 3);
            Assert.AreEqual(allExpectedSerialised, afterRequestResultSerialised);

            // Check results - GetBloodTestRequests with id expecting 2 results
            var afterRequestResultsById1 = _bloodTestService.GetBloodTestRequestsByPrescriptionId(1);
            var afterRequestResultsById1Serialised = Serialize(afterRequestResultsById1);

            Assert.IsTrue(afterRequestResultsById1.Count() == 2);
            Assert.AreEqual(expected1Serialised, afterRequestResultsById1Serialised);

            // Check results - GetBloodTestRequests with id expecting 1 result
            var afterRequestResultsById2 = _bloodTestService.GetBloodTestRequestsByPrescriptionId(2);
            var afterRequestResultsById2Serialised = Serialize(afterRequestResultsById2);

            Assert.IsTrue(afterRequestResultsById2.Count() == 1);
            Assert.AreEqual(expected2Serialised, afterRequestResultsById2Serialised);
        }

        // SetBloodTestDateTime test method
        [TestMethod]
        public void WhenSettingsAnAppointmentTimeTheRowIsUpdated()
        {
            // Check prior to any setup - via the database
            AssertBloodTestRequestsTableContainsXRows(0);

            // Prerequisite setup
            DateTime? initialTime = null;
            var updatedTime = DateTime.Now;

            var timeNow = DateTime.Now;
            var timeTomorrow = DateTime.Now.AddDays(1);

            var prescription = new Prescription
            {
                Id = 1,
                MedicationId = 1,
                PatientId = "1",
                Dosage = 1,
                DateStart = timeNow,
                DateEnd = timeTomorrow,
                PrescriptionStatus = PrescriptionStatus.Approved,
                IssueFrequency = IssueFrequency.Monthly
            };

            _bloodTestService.RequestBloodTestAsync(prescription, 1);

            // Begin actual test
            // Prep expected
            IEnumerable<BloodTestRequest> expectBeforeUpdate = new List<BloodTestRequest>
            {
                new BloodTestRequest
                {
                    Id = 1,
                    PrescriptionId = 1,
                    BloodTestId = 1,
                    AppointmentTime = null,
                    BloodTestStatus = BloodTestRequestStatus.Pending
                }
            };

            var beforeExpectedSerialised = Serialize(expectBeforeUpdate);

            IEnumerable<BloodTestRequest> expectedAfterUpdate = new List<BloodTestRequest>
            {
                new BloodTestRequest
                {
                    Id = 1,
                    PrescriptionId = 1,
                    BloodTestId = 1,
                    AppointmentTime = updatedTime,
                    BloodTestStatus = BloodTestRequestStatus.Scheduled
                }
            };

            var afterExpectedSerialised = Serialize(expectedAfterUpdate);

            // Check prior to adding - via the database
            AssertBloodTestRequestsTableContainsXRows(1);

            // Check prior to adding
            var beforeUpdateResults = _bloodTestService.GetBloodTestRequests();
            var beforeUpdateResultSerialised = Serialize(beforeUpdateResults);

            Assert.IsTrue(beforeUpdateResults.Count() == 1);
            Assert.AreEqual(beforeExpectedSerialised, beforeUpdateResultSerialised);
            Assert.AreEqual(initialTime, expectBeforeUpdate.First().AppointmentTime);

            // Update time
            var affectedRows = _bloodTestService.SetBloodTestDateTimeAsync(prescription, 1, updatedTime).Result;
            Assert.AreEqual(1, affectedRows);

            // Check results to adding - via the database
            AssertBloodTestRequestsTableContainsXRows(1);

            // Check results - GetBloodTestRequests with no id
            var afterUpdateResults = _bloodTestService.GetBloodTestRequests();
            var afterUpdateResultSerialised = Serialize(afterUpdateResults);

            Assert.IsTrue(afterUpdateResults.Count() == 1);
            Assert.AreEqual(afterExpectedSerialised, afterUpdateResultSerialised);
            Assert.AreEqual(updatedTime, expectedAfterUpdate.First().AppointmentTime);

            // Check results - GetBloodTestRequests with id
            var afterUpdateResultsById = _bloodTestService.GetBloodTestRequests(1);
            var afterUpdateResultByIdSerialised = Serialize(afterUpdateResultsById);

            Assert.IsTrue(afterUpdateResultsById.Count() == 1);
            Assert.AreEqual(afterExpectedSerialised, afterUpdateResultByIdSerialised);
            Assert.AreEqual(updatedTime, expectedAfterUpdate.First().AppointmentTime);
        }

        // SetBloodTestResults test method
        [TestMethod]
        public void WhenAddingBloodTestResultsRowsAreAdded()
        {
            // Check prior to any setup - via the database
            AssertBloodTestResultsTableContainsXRows(0);

            // Prerequisite setup
            var timeNow = DateTime.Now;
            var timeTomorrow = DateTime.Now.AddDays(1);

            var prescription = new Prescription
            {
                Id = 1,
                MedicationId = 1,
                PatientId = "1",
                Dosage = 1,
                DateStart = timeNow,
                DateEnd = timeTomorrow,
                PrescriptionStatus = PrescriptionStatus.Approved,
                IssueFrequency = IssueFrequency.Monthly
            };

            _bloodTestService.RequestBloodTestAsync(prescription, 1);
            _bloodTestService.RequestBloodTestAsync(prescription, 2);

            // Check the blood test requests have been set to complete
            var beforeBloodTestRequests = _bloodTestService.GetBloodTestRequests();
            var beforeBloodTestRequestsList = beforeBloodTestRequests.ToList();

            Assert.AreEqual(BloodTestRequestStatus.Pending, beforeBloodTestRequestsList.ElementAt(0).BloodTestStatus);
            Assert.AreEqual(BloodTestRequestStatus.Pending, beforeBloodTestRequestsList.ElementAt(1).BloodTestStatus);

            // Begin actual test
            var beforeResultResults = _bloodTestService.GetBloodTestResults().ToList();
            Assert.IsTrue(!beforeResultResults.Any());

            // Prep expected
            IEnumerable<BloodTestResult> allExpected = new List<BloodTestResult>
            {
                new BloodTestResult
                {
                    Id = 1,
                    BloodTestRequestId = 1,
                    Result = false,
                    ResultTime = timeNow
                },
                new BloodTestResult
                {
                    Id = 2,
                    BloodTestRequestId = 2,
                    Result = true,
                    ResultTime = timeNow
                }
            };

            IEnumerable<BloodTestResult> expected1 = new List<BloodTestResult>
            {
                new BloodTestResult
                {
                    Id = 2,
                    BloodTestRequestId = 2,
                    Result = true,
                    ResultTime = timeNow
                }
            };

            var allExpectedSerialised = Serialize(allExpected);

            var expected1Serialised = Serialize(expected1);

            // Check prior to adding blood tests - via the database
            AssertBloodTestResultsTableContainsXRows(0);

            // Set blood test results
            var affectedRows1 = _bloodTestService.SetBloodTestResults(1, false, timeNow);
            Assert.AreEqual(1, affectedRows1);

            var affectedRows2 = _bloodTestService.SetBloodTestResults(2, true, timeNow);
            Assert.AreEqual(1, affectedRows2);

            // Check prior to adding blood tests - via the database
            AssertBloodTestResultsTableContainsXRows(2);

            // Check results - GetBloodTestResults with no id
            var afterResultResults = _bloodTestService.GetBloodTestResults();
            var afterResultResultsSerialised = Serialize(afterResultResults);

            Assert.IsTrue(afterResultResults.Count() == 2);
            Assert.AreEqual(allExpectedSerialised, afterResultResultsSerialised);

            // Check results - GetBloodTestResults with id
            var afterResultResultsById = _bloodTestService.GetBloodTestResults(2);
            var afterResultResultsByIdSerialised = Serialize(afterResultResultsById);

            Assert.IsTrue(afterResultResultsById.Count() == 1);
            Assert.AreEqual(expected1Serialised, afterResultResultsByIdSerialised);

            // Check the blood test requests have been set to complete
            var afterBloodTestRequests = _bloodTestService.GetBloodTestRequests();
            var afterBloodTestRequestsList = afterBloodTestRequests.ToList();

            Assert.AreEqual(BloodTestRequestStatus.Complete, afterBloodTestRequestsList.ElementAt(0).BloodTestStatus);
            Assert.AreEqual(BloodTestRequestStatus.Complete, afterBloodTestRequestsList.ElementAt(1).BloodTestStatus);
        }

        [TestMethod]
        public void WhenSettingAnBloodTestRequestStatusTheRowIsUpdated()
        {
            // Check prior to any setup - via the database
            AssertBloodTestResultsTableContainsXRows(0);

            // Prerequisite setup
            var timeNow = DateTime.Now;
            var timeTomorrow = DateTime.Now.AddDays(1);

            var prescription = new Prescription
            {
                Id = 1,
                MedicationId = 1,
                PatientId = "1",
                Dosage = 1,
                DateStart = timeNow,
                DateEnd = timeTomorrow,
                PrescriptionStatus = PrescriptionStatus.Approved,
                IssueFrequency = IssueFrequency.Monthly
            };

            _bloodTestService.RequestBloodTestAsync(prescription, 1);

            // Check the blood test requests have been set to complete
            var beforeBloodTestRequests = _bloodTestService.GetBloodTestRequests();

            Assert.AreEqual(BloodTestRequestStatus.Pending, beforeBloodTestRequests.ElementAt(0).BloodTestStatus);

            // Update the status
            var result = _bloodTestService.SetBloodTestRequestStatus(1, BloodTestRequestStatus.Cancelled);
            Assert.AreEqual(1, result);

            // Assert value is updated
            var afterBloodTestRequests = _bloodTestService.GetBloodTestRequests();

            Assert.AreEqual(BloodTestRequestStatus.Cancelled, afterBloodTestRequests.ElementAt(0).BloodTestStatus);
        }

        // CancelBloodTestRequest test method
        [TestMethod]
        public void WhenCancellingBloodTestRequest()
        {
            // Check prior to any setup - via the database
            AssertBloodTestRequestsTableContainsXRows(0);

            // Prerequisite setup
            var timeNow = DateTime.Now;
            var timeTomorrow = DateTime.Now.AddDays(1);

            var prescription = new Prescription
            {
                Id = 1,
                MedicationId = 1,
                PatientId = "1",
                Dosage = 1,
                DateStart = timeNow,
                DateEnd = timeTomorrow,
                PrescriptionStatus = PrescriptionStatus.Approved,
                IssueFrequency = IssueFrequency.Monthly
            };

            _bloodTestService.RequestBloodTestAsync(prescription, 1);

            // Check row was inserted
            AssertBloodTestRequestsTableContainsXRows(1);

            var result = _bloodTestService.CancelBloodTestRequest(1);
            Assert.AreEqual(result, 1);

            //Check that the blood test request has been cancelled
            var result2 = _bloodTestService.GetBloodTestRequests(1).ElementAt(0);
            Assert.AreEqual(result2.BloodTestStatus, BloodTestRequestStatus.Cancelled); // this doesnt work but should
        }

        private void AssertBloodTestRequestsTableContainsXRows(int expectedRows)
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.ProgramData);
            
            // Check prior to make sure there are no prescription_collections
            var databaseResults = _databaseService.ExecuteScalarQuery("SELECT COUNT(*) FROM BloodTestRequests");
            var methodResults = _bloodTestService.GetBloodTestRequests();

            Assert.AreEqual(methodResults.Count(), databaseResults);
            Assert.AreEqual(expectedRows, databaseResults);
        }

        private void AssertBloodTestResultsTableContainsXRows(int expectedRows)
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.ProgramData);
            
            // Check prior to make sure there are no prescription_collections
            var databaseResults = _databaseService.ExecuteScalarQuery("SELECT COUNT(*) FROM BloodTestResults");
            var methodResults = _bloodTestService.GetBloodTestResults();

            Assert.AreEqual(expectedRows, databaseResults);
            Assert.AreEqual(methodResults.Count(), databaseResults);
        }
    }
}