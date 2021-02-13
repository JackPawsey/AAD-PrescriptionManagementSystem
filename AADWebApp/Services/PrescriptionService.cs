using System;
using System.Collections.Generic;
using AADWebApp.Models;
using Microsoft.AspNetCore.Hosting;

namespace AADWebApp.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        public IEnumerable<Prescription> GetPrescriptions()
        {
            IEnumerable<Prescription> prescriptions;

            try
            {
                //GET PRESCRIPTIONS TABLE
                prescriptions = null; //*** TEMPORARY ***
            }
            catch
            {
                prescriptions = null;
            }

            return prescriptions;
        }

        public string CreatePrescription(int patientId, string medicationType, int dosage, DateTime prescriptionEnd)
        {
            string result;

            try
            {
                //CREATE PRESCRIPTION TABLE ROW
                result = "Prescription created successfully";
            }
            catch
            {
                result = "Error prescription not created";
            }

            return result;
        }

        public string CancelPrescription(int prescriptionId)
        {
            string result;

            try
            {
                //DELETE PRESCRIPTION TABLE ROW or SET PrescriptionEnd to current date/time?
                //UPDATE PRESCRIPTION TABLE ROW
                result = "Task failed successfully";
            }
            catch
            {
                result = "Error prescription not cancelled";
            }

            return result;
        }

        public string SetPrescriptionMedication(int prescriptionId, int dosage)
        {
            string result;

            try
            {
                //UPDATE PRESCRIPTION TABLE ROW
                result = "Prescription medication updated successfully";
            }
            catch
            {
                result = "Error prescription medication not updated";
            }

            return result;
        }

        public string SetPrescriptionCollectionDateTime(int prescriptionId, DateTime dateTime)
        {
            string result;

            try
            {
                //UPDATE PRESCRIPTION TABLE ROW
                result = "Prescription collection date/time updated successfully";
            }
            catch
            {
                result = "Error prescription collection date/time not updated";
            }

            return result;
        }

        public string SetPrescriptionStatus(int prescriptionId, string status)
        {
            string result;

            try
            {
                //UPDATE PRESCRIPTION TABLE ROW
                result = "Prescription status updated successfully";
            }
            catch
            {
                result = "Error prescription status not updated";
            }

            return result;
        }
    }
}