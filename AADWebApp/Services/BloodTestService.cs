using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AADWebApp.Interfaces;
using AADWebApp.Models;

namespace AADWebApp.Services
{
    public class BloodTestService : IBloodTestService
    {
        private readonly IDatabaseService _databaseService;
        private readonly INotificationService _notificationService;

        public enum BloodTestRequestStatus
        {
            Pending,
            Complete,
            Cancelled
        }

        public BloodTestService(IDatabaseService databaseService, INotificationService notificationService)
        {
            _databaseService = databaseService;
            _notificationService = notificationService;
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

        public IEnumerable<BloodTestRequest> GetBloodTestRequests(short? Id = null)
        {
            var bloodTestRequests = new List<BloodTestRequest>();

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            //GET blood_test_requests TABLE
            using var result = _databaseService.RetrieveTable("BloodTestRequests", "Id", Id);

            while (result.Read())
            {
                bloodTestRequests.Add(new BloodTestRequest
                {
                    Id = (short)result.GetValue(0),
                    PrescriptionId = (short)result.GetValue(1),
                    BloodTestId = (short)result.GetValue(2),
                    AppointmentTime = (DateTime)result.GetValue(3),
                    BloodTestStatus = (BloodTestRequestStatus)Enum.Parse(typeof(BloodTestRequestStatus), result.GetValue(4).ToString() ?? throw new InvalidOperationException())
                });
            }

            return bloodTestRequests.AsEnumerable();
        }

        public IEnumerable<BloodTestRequest> GetBloodTestRequestsByPrescriptionId(short? prescriptionId = null)
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
                    AppointmentTime = (DateTime) result.GetValue(3),
                    BloodTestStatus = (BloodTestRequestStatus)Enum.Parse(typeof(BloodTestRequestStatus), result.GetValue(4).ToString() ?? throw new InvalidOperationException())
                });
            }

            return bloodTestRequests.AsEnumerable();
        }
        
        public async Task<int> RequestBloodTestAsync(Prescription prescription, int bloodTestId, DateTime appointmentTime) // We don't want to have to pass the prescription here but run into circular dependancy problems if not :(
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            var bloodTest = GetBloodTests((short?) bloodTestId).ElementAt(0);

            await _notificationService.SendBloodTestRequestNotification(prescription, bloodTest, DateTime.Now, appointmentTime);

            //CREATE BloodTestRequests TABLE ROW
            return _databaseService.ExecuteNonQuery($"INSERT INTO BloodTestRequests (PrescriptionId, BloodTestId, AppointmentTime, BloodTestStatus) VALUES ('{prescription.Id}', '{bloodTestId}', '{appointmentTime:yyyy-MM-dd HH:mm:ss}', '{BloodTestRequestStatus.Pending}')");
        }

        public async Task<int> SetBloodTestDateTimeAsync(Prescription prescription, int bloodTestRequestId, DateTime appointmentTime) // We don't want to have to pass the prescription here but run into circular dependancy problems if not :(
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            var bloodTestRequest = GetBloodTestRequests((short?) bloodTestRequestId).ElementAt(0);

            await _notificationService.SendBloodTestTimeUpdateNotification(prescription, bloodTestRequest, appointmentTime);

            //UPDATE BloodTestRequests TABLE ROW appointmentTime COLUMN
            return _databaseService.ExecuteNonQuery($"UPDATE BloodTestRequests SET AppointmentTime = '{appointmentTime:yyyy-MM-dd HH:mm:ss}' WHERE Id = '{bloodTestRequestId}'");
        }

        public int SetBloodTestResults(int bloodRequestTestId, bool result, DateTime resultTime)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            //CREATE BloodTestResults TABLE ROW
            return _databaseService.ExecuteNonQuery($"INSERT INTO BloodTestResults (BloodTestRequestId, TestResult, ResultTime) VALUES ('{bloodRequestTestId}', {(result ? 1 : 0)}, '{resultTime:yyyy-MM-dd HH:mm:ss}')");
        }

        public int SetBloodTestRequestStatus(int id, BloodTestRequestStatus bloodTestRequestStatus)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            //UPDATE BloodTestRequests TABLE ROW
            return _databaseService.ExecuteNonQuery($"UPDATE BloodTestRequests SET BloodTestStatus = '{bloodTestRequestStatus}' WHERE Id = '{id}'");
        }

        public int CancelBloodTestRequest(int id)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            //UPDATE BloodTestRequests TABLE ROW
            return _databaseService.ExecuteNonQuery($"UPDATE BloodTestRequests SET BloodTestStatus = '{BloodTestRequestStatus.Cancelled}' WHERE Id = '{id}'");
        }
    }
}