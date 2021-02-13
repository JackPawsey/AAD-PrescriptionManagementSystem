using AADWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AADWebApp.Services
{
    interface IBloodTestService
    {
        public IEnumerable<BloodTest> GetBloodTests();
        public string RequestBloodTest(int patientId, string bloodTestType);
        public string SetBloodTestDateTime(int bloodTestId, DateTime dateTime);
        public string SetBloodTestResults(BloodTestResult testResult);
    }
}
