using System;
using System.Collections.Generic;
using System.Linq;
using AADWebApp.Interfaces;
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

        public IEnumerable<BloodTest> GetBloodTests(short? id = null)
        {
            List<BloodTest> bloodTests = new List<BloodTest>();

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //GET blood_tests TABLE
            var result = _databaseService.RetrieveTable("blood_tests", "id", id);

            while (result.Read())
            {
                bloodTests.Add(new BloodTest
                {
                    id = (Int16)result.GetValue(0),
                    abbreviated_title = (string)result.GetValue(1),
                    full_title = (string)result.GetValue(2),
                    restriction_level = Convert.IsDBNull(result.GetValue(3)) ? (short?)null : (Int16)result.GetValue(3)
                });
            }

            return bloodTests.AsEnumerable();
        }

        public IEnumerable<BloodTestResult> GetBloodTestResults(short? bloodTestId = null)
        {
            List<BloodTestResult> BloodTestResults = new List<BloodTestResult>();

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //GET blood_test_results TABLE
            var result = _databaseService.RetrieveTable("blood_test_results", "blood_test_id", bloodTestId);

            while (result.Read())
            {
                BloodTestResults.Add(new BloodTestResult
                {
                    id = (Int16)result.GetValue(0),
                    blood_test_id = (Int16)result.GetValue(1),
                    result = (bool)result.GetValue(2),
                    resultTime = (DateTime)result.GetValue(3)
                });
            }

            return BloodTestResults.AsEnumerable();
        }
        public IEnumerable<BloodTestRequest> GetBloodTestRequests(short? prescriptionId = null)
        {
            List<BloodTestRequest> BloodTestRequests = new List<BloodTestRequest>();

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //GET blood_test_requests TABLE
            var result = _databaseService.RetrieveTable("blood_test_requests", "prescription_id", prescriptionId);

            while (result.Read())
            {
                BloodTestRequests.Add(new BloodTestRequest
                {
                    id = (Int16)result.GetValue(0),
                    prescription_id = (Int16)result.GetValue(1),
                    blood_test_id = (Int16)result.GetValue(2),
                    appointment_time = (DateTime)result.GetValue(3)
                });
            }

            return BloodTestRequests.AsEnumerable();
        }


        public int RequestBloodTest(int prescriptionId, int bloodTestId, DateTime appointmentTime)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //CREATE blood_test_requests TABLE ROW
            return _databaseService.ExecuteNonQuery($"INSERT INTO blood_test_requests (prescription_id, blood_test_id, appointment_time) VALUES ({prescriptionId}, {bloodTestId}, '{appointmentTime.ToString("yyyy-MM-dd HH:mm:ss")}')");
        }

        public int SetBloodTestDateTime(int id, DateTime appointmentTime)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //UPDATE blood_test_requests TABLE ROW appointmentTime COLUMN
            return _databaseService.ExecuteNonQuery($"UPDATE blood_test_requests SET appointment_time = '{appointmentTime.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE id = {id}");
        }

        public int SetBloodTestResults(int bloodTestId, bool result, DateTime resultTime)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //CREATE blood_test_results TABLE ROW
            return _databaseService.ExecuteNonQuery($"INSERT INTO blood_test_results (blood_test_id, result, result_time) VALUES ({bloodTestId}, {(result ? 1 : 0)}, '{resultTime.ToString("yyyy-MM-dd HH:mm:ss")}')");
        }
    }
}