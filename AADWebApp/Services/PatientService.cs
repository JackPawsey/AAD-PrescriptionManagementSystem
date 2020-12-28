using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;

namespace TestWebApp.Services
{
    public class PatientService
    {
        public IWebHostEnvironment WebHostEnvironment { get; }

        public PatientService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<int> GetPatients()
        {
            IEnumerable<int> Patients = null;

            //CALL DATABASE METHOD TO GET PATIENTS

            return Patients;
        }

        public String SetCommunicationPreferences(int PatientID, String CommunicationPreferences)
        {
            String Result = "";

            return Result;
        }
    }
}
