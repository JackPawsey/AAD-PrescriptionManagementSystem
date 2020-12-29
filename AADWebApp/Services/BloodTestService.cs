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
            String Result;

            try
            {
                //CREATE BLOODTEST TABLE ROW
                Result = "Blood test request created succuessfully";
            }
            catch
            {
                Result = "Error blood test request not created";
            }

            return Result;
        }

        public String SetBloodTestDateTime(int BloodTestID, DateTime DateTime)
        {
            String Result;

            try
            {
                //UPDATE BLOODTEST TABLE ROW
                Result = "Blood test date/time updated succuessfully";
            }
            catch
            {
                Result = "Error blood test date/time not updated";
            }

            return Result;
        }

        public String SetBloodTestResults(BloodTestResult TestResult)
        {
            String Result;

            try
            {
                //UPDATE BLOODTEST TABLE ROW
                Result = "Blood test results updated succuessfully";
            }
            catch
            {
                Result = "Error blood test results not updated";
            }

            return Result;
        }
    }
}
