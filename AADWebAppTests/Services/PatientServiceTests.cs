using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using AADWebApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static AADWebApp.Services.DatabaseService;
using static AADWebApp.Services.PatientService;

namespace AADWebAppTests.Services
{
    [DoNotParallelize]
    [TestClass]
    public class PatientServiceTests : TestBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IPatientService _patientService;

        public PatientServiceTests()
        {
            _databaseService = Get<IDatabaseService>();
            _patientService = new PatientService(_databaseService);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.program_data);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.program_data);
            _databaseService.ExecuteNonQuery($"DELETE FROM patients;");
        }

        [TestMethod]
        public void WhenThereAreNoPatients()
        {
            var databaseResults = _databaseService.ExecuteScalarQuery("SELECT COUNT(*) FROM patients");
            var methodResults = _patientService.GetPatients();

            var enumerable = methodResults.ToList();

            Assert.IsTrue(!enumerable.Any());
            Assert.AreEqual(enumerable.Count(), databaseResults);
        }

        [TestMethod]
        public void WhenGettingPatients()
        {
            AssertPatientsTableContainsXRows(0);

            // Prep expected
            IEnumerable<Patient> allExpected = new List<Patient>
            {
                new Patient
                {
                    Id = "some-guid-1",
                    CommunicationPreferences = CommunicationPreferences.Email,
                    NhsNumber = "1",
                    GeneralPractitioner = "some-gp-1"
                },
                new Patient
                {
                    Id = "some-guid-2",
                    CommunicationPreferences = CommunicationPreferences.Sms,
                    NhsNumber = "2",
                    GeneralPractitioner = "some-gp-2"
                },
                new Patient
                {
                    Id = "some-guid-3",
                    CommunicationPreferences = CommunicationPreferences.Both,
                    NhsNumber = "3",
                    GeneralPractitioner = "some-gp-3"
                }
            };

            IEnumerable<Patient> singleExpected = new List<Patient>
            {
                new Patient
                {
                    Id = "some-guid-2",
                    CommunicationPreferences = CommunicationPreferences.Sms,
                    NhsNumber = "2",
                    GeneralPractitioner = "some-gp-2"
                }
            };

            var allExpectedSerialised = JsonConvert.SerializeObject(allExpected);
            var singleSerialised = JsonConvert.SerializeObject(singleExpected);

            // Add patients
            var affectedRows1 = _patientService.CreateNewPatientEntry("some-guid-1", CommunicationPreferences.Email, "1", "some-gp-1");
            Assert.AreEqual(1, affectedRows1);

            var affectedRows2 = _patientService.CreateNewPatientEntry("some-guid-2", CommunicationPreferences.Sms, "2", "some-gp-2");
            Assert.AreEqual(1, affectedRows2);

            var affectedRows3 = _patientService.CreateNewPatientEntry("some-guid-3", CommunicationPreferences.Both, "3", "some-gp-3");
            Assert.AreEqual(1, affectedRows3);

            // Check amount of database rows
            var databaseRows = _databaseService.ExecuteScalarQuery("SELECT COUNT(*) FROM patients");
            Assert.IsTrue(databaseRows == 3);

            // Check results - GetPatients with no id
            var afterCreateResults = _patientService.GetPatients();
            var afterCreateResultsSerialised = JsonConvert.SerializeObject(afterCreateResults);

            Assert.IsTrue(afterCreateResults.Count() == 3);
            Assert.AreEqual(allExpectedSerialised, afterCreateResultsSerialised);

            // Check results - GetPatients with valid id
            var afterCreateResultsByValidId = _patientService.GetPatients("some-guid-2");
            var afterCreateResultsByValidIdSerialised = JsonConvert.SerializeObject(afterCreateResultsByValidId);

            Assert.IsTrue(afterCreateResultsByValidId.Count() == 1);
            Assert.AreEqual(singleSerialised, afterCreateResultsByValidIdSerialised);

            // Check results - GetPatients with invalid id
            var afterCreateResultsByInvalidId = _patientService.GetPatients("some-invalid-guid");
            var afterCreateResultsByInvalidIdSerialised = JsonConvert.SerializeObject(afterCreateResultsByInvalidId);
            var expectedInvalidIdSerialised = JsonConvert.SerializeObject(new List<Patient>());

            Assert.IsTrue(!afterCreateResultsByInvalidId.Any());
            Assert.AreEqual(expectedInvalidIdSerialised, afterCreateResultsByInvalidIdSerialised);
        }

        [TestMethod]
        [ExpectedException(typeof(SqlException), "A duplicate NHS number was permitted to be added to the database.")]
        public void WhenInsertingDuplicateNhsNumbersItFails()
        {
            AssertPatientsTableContainsXRows(0);

            // Add first patient
            var affectedRows1 = _patientService.CreateNewPatientEntry("some-guid-1", CommunicationPreferences.Email, "1", "some-gp-1");
            Assert.AreEqual(1, affectedRows1);

            // Check amount of database rows
            var databaseRows1 = _databaseService.ExecuteScalarQuery("SELECT COUNT(*) FROM patients");
            Assert.IsTrue(databaseRows1 == 1);

            // Add second patient, which should throw an SQL exception
            _patientService.CreateNewPatientEntry("some-guid-2", CommunicationPreferences.Email, "1", "some-gp-1");
        }

        [TestMethod]
        public void WhenSettingCommunicationPreferenceItIsUpdated()
        {
            AssertPatientsTableContainsXRows(0);

            // Prep expected
            var originalExpected = AddSinglePatient("some-guid-1", CommunicationPreferences.Email, "1", "some-gp-1");

            IEnumerable<Patient> expectedAfterUpdate = new List<Patient>
            {
                new Patient
                {
                    Id = "some-guid-1",
                    CommunicationPreferences = CommunicationPreferences.Sms,
                    NhsNumber = "1",
                    GeneralPractitioner = "some-gp-1"
                }
            };

            var originalExpectedSerialised = JsonConvert.SerializeObject(originalExpected);
            var updatedExpectedSerialised = JsonConvert.SerializeObject(expectedAfterUpdate);

            // Make sure they're not the same yet (as we haven't updated)
            Assert.AreNotEqual(originalExpectedSerialised, updatedExpectedSerialised);

            // Update
            var affectedRows = _patientService.SetCommunicationPreferences("some-guid-1", CommunicationPreferences.Sms);
            Assert.AreEqual(1, affectedRows);

            // Check there's one database row
            AssertPatientsTableContainsXRows(1);

            // Check results via GetPatients with id
            var afterUpdateResults = _patientService.GetPatients("some-guid-1");
            var afterUpdateResultsSerialised = JsonConvert.SerializeObject(afterUpdateResults);

            Assert.AreEqual(updatedExpectedSerialised, afterUpdateResultsSerialised);
        }

        [TestMethod]
        public void WhenUpdatingGeneralPractitionerItIsUpdated()
        {
            AssertPatientsTableContainsXRows(0);

            // Prep expected
            var originalExpected = AddSinglePatient("some-guid-1", CommunicationPreferences.Email, "1", "some-gp-1");

            IEnumerable<Patient> expectedAfterUpdate = new List<Patient>
            {
                new Patient
                {
                    Id = "some-guid-1",
                    CommunicationPreferences = CommunicationPreferences.Email,
                    NhsNumber = "1",
                    GeneralPractitioner = "some-gp-2"
                }
            };

            var originalExpectedSerialised = JsonConvert.SerializeObject(originalExpected);
            var updatedExpectedSerialised = JsonConvert.SerializeObject(expectedAfterUpdate);

            // Make sure they're not the same yet (as we haven't updated)
            Assert.AreNotEqual(originalExpectedSerialised, updatedExpectedSerialised);

            // Update
            var affectedRows = _patientService.UpdateGeneralPractitioner("some-guid-1", "some-gp-2");
            Assert.AreEqual(1, affectedRows);

            // Check there's one database row
            AssertPatientsTableContainsXRows(1);

            // Check results via GetPatients with id
            var afterUpdateResults = _patientService.GetPatients("some-guid-1");
            var afterUpdateResultsSerialised = JsonConvert.SerializeObject(afterUpdateResults);

            Assert.AreEqual(updatedExpectedSerialised, afterUpdateResultsSerialised);
        }

        [TestMethod]
        public void WhenCreatingANewPatientARowIsAdded()
        {
            AssertPatientsTableContainsXRows(0);

            // Prep expected
            var expected = AddSinglePatient("some-guid-1", CommunicationPreferences.Email, "1", "some-gp-1");
            var expectedSerialised = JsonConvert.SerializeObject(expected);

            // Check the entry matches the expected
            var thePatient = _patientService.GetPatients("some-guid-1");
            var thePatientSerialised = JsonConvert.SerializeObject(thePatient);

            Assert.AreEqual(expectedSerialised, thePatientSerialised);
        }

        private void AssertPatientsTableContainsXRows(int expectedRows)
        {
            // Check prior to make sure there are no patients
            var databaseResults = _databaseService.ExecuteScalarQuery("SELECT COUNT(*) FROM patients");
            var methodResults = _patientService.GetPatients();

            Assert.AreEqual(methodResults.Count(), databaseResults);
            Assert.AreEqual(expectedRows, databaseResults);
        }

        private IEnumerable<Patient> AddSinglePatient(string id, CommunicationPreferences communicationPreferences, string nhsNumber, string generalPractitioner)
        {
            IEnumerable<Patient> expected = new List<Patient>
            {
                new Patient
                {
                    Id = id,
                    CommunicationPreferences = communicationPreferences,
                    NhsNumber = nhsNumber,
                    GeneralPractitioner = generalPractitioner
                }
            };

            // Add patient and verify
            var affectedRows1 = _patientService.CreateNewPatientEntry(id, communicationPreferences, nhsNumber, generalPractitioner);
            Assert.AreEqual(1, affectedRows1);

            // Check amount of database rows
            AssertPatientsTableContainsXRows(1);

            return expected;
        }
    }
}