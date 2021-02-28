using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AADWebApp.Models;

namespace AADWebApp.Interfaces
{
    public interface IBloodTestService
    {
        public IEnumerable<BloodTest> GetBloodTests(short? id = null);
        public IEnumerable<BloodTestResult> GetBloodTestResults(short? bloodTestRequestId = null);
        public IEnumerable<BloodTestRequest> GetBloodTestRequests(short? prescriptionId = null);
        public Task<int> RequestBloodTestAsync(Prescription prescription, int bloodTestId, DateTime appointmentTime);
        public int SetBloodTestDateTime(int id, DateTime appointmentTime);
        public int SetBloodTestResults(int bloodRequestTestId, bool result, DateTime resultTime);
        public int DeleteBloodTestRequest(int id);
    }
}