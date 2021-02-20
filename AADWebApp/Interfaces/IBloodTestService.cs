﻿using AADWebApp.Models;
using System;
using System.Collections.Generic;

namespace AADWebApp.Interfaces
{
    public interface IBloodTestService
    {
        public IEnumerable<BloodTest> GetBloodTests(short? id = null);
        public IEnumerable<BloodTestResult> GetBloodTestResults(short? bloodTestId = null);
        public IEnumerable<BloodTestRequest> GetBloodTestRequests(short? prescriptionId = null);
        public int RequestBloodTest(int prescriptionId, int bloodTestId, DateTime appointmentTime);
        public int SetBloodTestDateTime(int id, DateTime appointmentTime);
        public int SetBloodTestResults(int bloodTestId, bool result, DateTime resultTime);
    }
}
