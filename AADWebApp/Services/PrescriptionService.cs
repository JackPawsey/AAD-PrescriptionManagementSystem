using AADWebApp.Models;
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

        public IEnumerable<Prescription> GetPrescriptions()
        {
            IEnumerable<Prescription> Prescriptions;

            try
            {
                //GET PRESCRIPTIONS TABLE
                Prescriptions = null; //*** TEMPORARY ***
            }
            catch
            {
                Prescriptions = null;
            }

            return Prescriptions;
        }

        public String CreatePrescription(int PatientID, String MedicationType, int Dosage, DateTime PrescriptionEnd)
        {
            String Result;

            try
            {
                //CREATE PRESCRIPTION TABLE ROW
                Result = "Prescription created succuessfully";
            }
            catch
            {
                Result = "Error prescription not created";
            }

            return Result;
        }

        public String CancelPrescription(int PrescriptionID)
        {
            String Result;

            try
            {
                //DELETE PRESCRIPTION TABLE ROW or SET PrescriptionEnd to currenct date/time?
                //UPDATE PRESCRIPTION TABLE ROW
                Result = "Task failed succuessfully";
            }
            catch
            {
                Result = "Error prescription not cancelled";
            }

            return Result;
        }

        public String SetPrescriptionMedication(int PrescriptionID, int Doasge)
        {
            String Result;

            try
            {
                //UPDATE PRESCRIPTION TABLE ROW
                Result = "Prescription medication updated succuessfully";
            }
            catch
            {
                Result = "Error prescription medication not updated";
            }

            return Result;
        }

        public String SetPrescriptionCollectionDateTime(int PrescriptionID, DateTime DateTime)
        {
            String Result;

            try
            {
                //UPDATE PRESCRIPTION TABLE ROW
                Result = "Prescription collection date/time updated succuessfully";
            }
            catch
            {
                Result = "Error prescription collection date/time not updated";
            }

            return Result;
        }

        public String SetPrescriptionStatus(int PrescriptionID, String Status)
        {
            String Result;

            try
            {
                //UPDATE PRESCRIPTION TABLE ROW
                Result = "Prescription status updated succuessfully";
            }
            catch
            {
                Result = "Error prescription status not updated";
            }

            return Result;
        }
    }
}
