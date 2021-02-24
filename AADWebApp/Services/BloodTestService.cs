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

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            //GET blood_tests TABLE
            using var result = _databaseService.RetrieveTable("BloodTests", "Id", id);

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

        public IEnumerable<BloodTestResult> GetBloodTestResults(short? bloodTestRequestId = null)
        {
            var bloodTestResults = new List<BloodTestResult>();

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            //GET BloodTestResults TABLE
            using var result = _databaseService.RetrieveTable("BloodTestResults", "BloodTestRequestId", bloodTestRequestId);

            while (result.Read())
            {
                bloodTestResults.Add(new BloodTestResult
                {
                    Id = (short) result.GetValue(0),
                    BloodTestRequestId = (short) result.GetValue(1),
                    Result = (bool) result.GetValue(2),
                    ResultTime = (DateTime) result.GetValue(3)
                });
            }

            return bloodTestResults.AsEnumerable();
        }

        public IEnumerable<BloodTestRequest> GetBloodTestRequests(short? prescriptionId = null)
        {
            var bloodTestRequests = new List<BloodTestRequest>();

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            //GET blood_test_requests TABLE
            using var result = _databaseService.RetrieveTable("BloodTestRequests", "PrescriptionId", prescriptionId);

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
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            //CREATE BloodTestRequests TABLE ROW
            return _databaseService.ExecuteNonQuery($"INSERT INTO BloodTestRequests (PrescriptionId, BloodTestId, AppointmentTime) VALUES ('{prescriptionId}', '{bloodTestId}', '{appointmentTime:yyyy-MM-dd HH:mm:ss}')");
        }

        public int SetBloodTestDateTime(int id, DateTime appointmentTime)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            //UPDATE BloodTestRequests TABLE ROW appointmentTime COLUMN
            return _databaseService.ExecuteNonQuery($"UPDATE BloodTestRequests SET AppointmentTime = '{appointmentTime:yyyy-MM-dd HH:mm:ss}' WHERE Id = '{id}'");
        }

        public int SetBloodTestResults(int bloodTestId, bool result, DateTime resultTime)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            //CREATE BloodTestResults TABLE ROW
            return _databaseService.ExecuteNonQuery($"INSERT INTO BloodTestResults (BloodTestRequestId, TestResult, ResultTime) VALUES ('{bloodTestId}', {(result ? 1 : 0)}, '{resultTime:yyyy-MM-dd HH:mm:ss}')");
        }
    }
}