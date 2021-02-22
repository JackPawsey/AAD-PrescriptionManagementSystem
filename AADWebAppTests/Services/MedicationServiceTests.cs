using System.Collections.Generic;
using System.Linq;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using AADWebApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AADWebAppTests.Services
{
    [TestClass]
    public class MedicationServiceTests : TestBase
    {
        private readonly IMedicationService _medicationService;

        public MedicationServiceTests()
        {
            _medicationService = new MedicationService(Get<IDatabaseService>());
        }

        [TestMethod]
        public void WhenThereAreMedications()
        {
            var results = _medicationService.GetMedications();

            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void WhenGettingMedicationById()
        {
            // Prep expected
            IEnumerable<Medication> expected = new List<Medication>
            {
                new Medication
                {
                    Id = 2,
                    MedicationName = "Olanzapine",
                    BloodWorkRestrictionLevel = 2
                }
            };

            var expectedSerialised = Serialize(expected);

            // GetMedications with id 2
            var results = _medicationService.GetMedications(2);

            // Check results
            var resultSerialised = Serialize(results);

            Assert.IsTrue(results.Count() == 1);
            Assert.AreEqual(expectedSerialised, resultSerialised);
        }
    }
}