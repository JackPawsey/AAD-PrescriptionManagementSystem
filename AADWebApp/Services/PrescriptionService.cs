using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;

namespace AADWebApp.Services
{
    public class PrescriptionService
    {
        public IWebHostEnvironment WebHostEnvironment { get; }

        public PrescriptionService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<int> GetPrescriptions()
        {
            IEnumerable<int> Prescriptions = null;

            return Prescriptions;
        }

        public String CreatePrescription(int PatientID, String MedicationType, int Dosage, DateTime PrescriptionEnd)
        {
            String Result = "";

            return Result;
        }

        public String CancelPrescription(int PrescriptionID)
        {
            String Result = "";

            return Result;
        }

        public String SetPrescriptionMedication(int PrescriptionID, int Doasge)
        {
            String Result = "";

            return Result;
        }

        public String SetPrescriptionCollectionDateTime(int PrescriptionID, DateTime DateTime)
        {
            String Result = "";

            return Result;
        }

        public String SetPrescriptionStatus(int PrescriptionID, String Status)
        {
            String Result = "";

            return Result;
        }
    }
}
