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
        public Task<int> RequestBloodTestAsync(Prescription prescription, short bloodTestId, DateTime appointmentTime);
        public Task<int> SetBloodTestDateTimeAsync(Prescription prescription, short bloodTestRequestId, DateTime appointmentTime);
        public int SetBloodTestResults(short bloodRequestTestId, bool result, DateTime resultTime);
        public int SetBloodTestRequestStatus(short id, BloodTestRequestStatus bloodTestRequestStatus);
        public int CancelBloodTestRequest(short id);
    }
}