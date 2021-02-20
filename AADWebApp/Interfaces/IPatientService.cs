using AADWebApp.Models;
using System.Collections.Generic;

namespace AADWebApp.Interfaces
{
    interface IPatientService
    {
        public IEnumerable<Patient> GetPatients();
        public string SetCommunicationPreferences(int patientId, string communicationPreferences);
    }
}
