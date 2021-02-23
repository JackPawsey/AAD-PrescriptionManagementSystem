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
            var bloodTests = new List<BloodTest>();

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //GET blood_tests TABLE
            using var result = _databaseService.RetrieveTable("blood_tests", "id", id);

            while (result.Read())
            {
                bloodTests.Add(new BloodTest
                {
                    Id = (short) result.GetValue(0),
                    AbbreviatedTitle = (string) result.GetValue(1),
                    FullTitle = (string) result.GetValue(2),
                    RestrictionLevel = Convert.IsDBNull(result.GetValue(3)) ? (short?) null : (short) result.GetValue(3)
                });
            }

            return bloodTests.AsEnumerable();
        }

        public IEnumerable<BloodTestResult> GetBloodTestResults(short? bloodTestId = null)
        {
            var bloodTestResults = new List<BloodTestResult>();

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //GET blood_test_results TABLE
            using var result = _databaseService.RetrieveTable("blood_test_results", "blood_test_id", bloodTestId);

            while (result.Read())
            {
                bloodTestResults.Add(new BloodTestResult
                {
                    Id = (short) result.GetValue(0),
                    BloodTestId = (short) result.GetValue(1),
                    Result = (bool) result.GetValue(2),
                    ResultTime = (DateTime) result.GetValue(3)
                });
            }

            return bloodTestResults.AsEnumerable();
        }

        public IEnumerable<BloodTestRequest> GetBloodTestRequests(short? prescriptionId = null)
        {
            var bloodTestRequests = new List<BloodTestRequest>();

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //GET blood_test_requests TABLE
            using var result = _databaseService.RetrieveTable("blood_test_requests", "prescription_id", prescriptionId);

            while (result.Read())
            {
                bloodTestRequests.Add(new BloodTestRequest
                {
                    Id = (short) result.GetValue(0),
                    PrescriptionId = (short) result.GetValue(1),
                    BloodTestId = (short) result.GetValue(2),
                    AppointmentTime = (DateTime) result.GetValue(3)
                });
            }

            return bloodTestRequests.AsEnumerable();
        }


        public int RequestBloodTest(int prescriptionId, int bloodTestId, DateTime appointmentTime)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //CREATE blood_test_requests TABLE ROW
            return _databaseService.ExecuteNonQuery($"INSERT INTO blood_test_requests (prescription_id, blood_test_id, appointment_time) VALUES ('{prescriptionId}', '{bloodTestId}', '{appointmentTime:yyyy-MM-dd HH:mm:ss}')");
        }

        public int SetBloodTestDateTime(int id, DateTime appointmentTime)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //UPDATE blood_test_requests TABLE ROW appointmentTime COLUMN
            return _databaseService.ExecuteNonQuery($"UPDATE blood_test_requests SET appointment_time = '{appointmentTime:yyyy-MM-dd HH:mm:ss}' WHERE id = '{id}'");
        }

        public int SetBloodTestResults(int bloodTestId, bool result, DateTime resultTime)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //CREATE blood_test_results TABLE ROW
            return _databaseService.ExecuteNonQuery($"INSERT INTO blood_test_results (blood_test_id, result, result_time) VALUES ('{bloodTestId}', {(result ? 1 : 0)}, '{resultTime:yyyy-MM-dd HH:mm:ss}')");
        }
    }
}