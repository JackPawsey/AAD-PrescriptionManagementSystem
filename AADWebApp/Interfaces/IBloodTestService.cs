using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AADWebApp.Models;
using static AADWebApp.Services.BloodTestService;

namespace AADWebApp.Interfaces
{
    public interface IBloodTestService
    {
        public IEnumerable<BloodTest> GetBloodTests(short? id = null);
        public IEnumerable<BloodTestResult> GetBloodTestResults(short? bloodTestRequestId = null);
        public IEnumerable<BloodTestRequest> GetBloodTestRequests(short? Id = null);
        public IEnumerable<BloodTestRequest> GetBloodTestRequestsByPrescriptionId(short? prescriptionId = null);
        public Task<int> RequestBloodTestAsync(Prescription prescription, int bloodTestId, DateTime appointmentTime);
        public Task<int> SetBloodTestDateTimeAsync(Prescription prescription, int bloodTestRequestId, DateTime appointmentTime);
        public int SetBloodTestResults(int bloodRequestTestId, bool result, DateTime resultTime);
        public int SetBloodTestRequestStatus(int id, BloodTestRequestStatus bloodTestRequestStatus);
        public int CancelBloodTestRequest(int id);
    }
}