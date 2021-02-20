using System;
using System.Collections.Generic;
using System.Linq;
using AADWebApp.Models;

namespace AADWebApp.Services
{
    public class BloodTestService : IBloodTestService
    {
        private readonly IDatabaseService _databaseService;


        public BloodTestService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<Medication> GetMedications()
        {
            List<Medication> medications = new List<Medication>();

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //GET BLOODTEST TABLE
            var result = _databaseService.RetrieveTable("medications");

            while (result.Read())
            {
                Medication medication = new Medication();

                medication.id = (Int16)result.GetValue(0);
                medication.medication = (string)result.GetValue(1);

                if (Convert.IsDBNull(result.GetValue(2)))
                {
                    medication.blood_work_restriction_level = null;
                }
                else
                {
                    medication.blood_work_restriction_level = (Int16)result.GetValue(2);
                }

                medications.Add(medication);
            }

            return medications.AsEnumerable();
        }

        public IEnumerable<BloodTest> GetBloodTests()
        {
            List<BloodTest> bloodTests = new List<BloodTest>();

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //GET BLOODTEST TABLE
            var result = _databaseService.RetrieveTable("blood_tests");

            while (result.Read())
            {
                BloodTest bloodTest = new BloodTest();

                bloodTest.id = (Int16)result.GetValue(0);
                bloodTest.abbreviated_title = (string)result.GetValue(1);
                bloodTest.full_title = (string)result.GetValue(2);

                if (Convert.IsDBNull(result.GetValue(3)))
                {
                    bloodTest.restriction_level = null;
                }
                else
                {
                    bloodTest.restriction_level = (Int16)result.GetValue(3);
                }

                bloodTests.Add(bloodTest);
            }

            return bloodTests.AsEnumerable();
        }


        public string RequestBloodTest(int patientId, string bloodTestType)
        {
            string result;

            try
            {
                //CREATE BLOODTEST TABLE ROW
                result = "Blood test request created successfully";
            }
            catch
            {
                result = "Error blood test request not created";
            }

            return result;
        }

        public string SetBloodTestDateTime(int bloodTestId, DateTime dateTime)
        {
            string result;

            try
            {
                //UPDATE BLOODTEST TABLE ROW
                result = "Blood test date/time updated successfully";
            }
            catch
            {
                result = "Error blood test date/time not updated";
            }

            return result;
        }

        public string SetBloodTestResults(BloodTestResult testResult)
        {
            string result;

            try
            {
                //UPDATE BLOODTEST TABLE ROW
                result = "Blood test results updated successfully";
            }
            catch
            {
                result = "Error blood test results not updated";
            }

            return result;
        }
    }
}