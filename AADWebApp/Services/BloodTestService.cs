using System;
using System.Collections.Generic;
using AADWebApp.Models;
using Microsoft.AspNetCore.Hosting;

namespace AADWebApp.Services
{
    public class BloodTestService : IBloodTestService
    {
        public IEnumerable<BloodTest> GetBloodTests()
        {
            IEnumerable<BloodTest> bloodTests;

            try
            {
                //GET BLOODTEST TABLE
                bloodTests = null; //*** TEMPORARY ***
            }
            catch
            {
                bloodTests = null;
            }

            return bloodTests;
        }

        public string RequestBloodTest(int patientId, string bloodTestType)
        {
            string result;

            try
            {
                //CREATE BLOODTEST TABLE ROW
                result = "Blood test request created successfully";
            }
            catch
            {
                result = "Error blood test request not created";
            }

            return result;
        }

        public string SetBloodTestDateTime(int bloodTestId, DateTime dateTime)
        {
            string result;

            try
            {
                //UPDATE BLOODTEST TABLE ROW
                result = "Blood test date/time updated successfully";
            }
            catch
            {
                result = "Error blood test date/time not updated";
            }

            return result;
        }

        public string SetBloodTestResults(BloodTestResult testResult)
        {
            string result;

            try
            {
                //UPDATE BLOODTEST TABLE ROW
                result = "Blood test results updated successfully";
            }
            catch
            {
                result = "Error blood test results not updated";
            }

            return result;
        }
    }
}