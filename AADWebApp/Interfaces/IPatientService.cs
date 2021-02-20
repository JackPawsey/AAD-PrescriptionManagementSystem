using System.Collections.Generic;
using AADWebApp.Models;

namespace AADWebApp.Interfaces
{
    public interface IPatientService
    {
        public IEnumerable<Patient> GetPatients();
        public string SetCommunicationPreferences(int patientId, string communicationPreferences);
    }
}