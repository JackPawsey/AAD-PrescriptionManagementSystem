using AADWebApp.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;

namespace AADWebApp.Services
{
    public class PatientService
    {
        public IWebHostEnvironment WebHostEnvironment { get; }

        public PatientService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<Patient> GetPatients()
        {
            IEnumerable<Patient> Patients;

            try
            {
                //GET PATIENT TABLE
                Patients = null; //*** TEMPORARY ***
            }
            catch
            {
                Patients = null;
            }

            return Patients;
        }

        public String SetCommunicationPreferences(int PatientID, String CommunicationPreferences)
        {
            String Result;

            try
            {
                //UPDATE PATIENT TABLE ROW
                Result = "Communication preferences updated succuessfully";
            }
            catch
            {
                Result = "Error communication preferences not updated";
            }

            return Result;
        }
    }
}
