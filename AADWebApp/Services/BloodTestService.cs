using AADWebApp.Models;
using Microsoft.AspNetCore.Hosting;
using System;

namespace AADWebApp.Services
{
    public class BloodTestService
    {
        public IWebHostEnvironment WebHostEnvironment { get; }

        public BloodTestService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public String RequestBloodTest(int PatientID, String BloodTestType)
        {
            String Result = "";

            return Result;
        }

        public String SetBloodTestDateTime(int BloodTestID, DateTime DateTime)
        {
            String Result = "";

            return Result;
        }

        public String SetBloodTestResults(BloodTestResult TestResult)
        {
            String Result = "";

            return Result;
        }
    }
}
