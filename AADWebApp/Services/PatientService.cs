using System.Collections.Generic;
using AADWebApp.Models;

namespace AADWebApp.Services
{
    public class PatientService
    {
        public IEnumerable<Patient> GetPatients()
        {
            IEnumerable<Patient> patients;

            try
            {
                //GET PATIENT TABLE
                patients = null; //*** TEMPORARY ***
            }
            catch
            {
                patients = null;
            }

            return patients;
        }

        public string SetCommunicationPreferences(int patientId, string communicationPreferences)
        {
            string result;

            try
            {
                //UPDATE PATIENT TABLE ROW
                result = "Communication preferences updated successfully";
            }
            catch
            {
                result = "Error communication preferences not updated";
            }

            return result;
        }
    }
}