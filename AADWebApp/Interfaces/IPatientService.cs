using System.Collections.Generic;
using AADWebApp.Models;
using static AADWebApp.Services.PatientService;

namespace AADWebApp.Interfaces
{
    public interface IPatientService
    {
        public IEnumerable<Patient> GetPatients(string? id = null);
        public int SetCommunicationPreferences(string patientId, CommunicationPreferences communicationPreferences);
        public int UpdateGeneralPractitioner(string patientId, string generalPractitionerId);
        public int CreateNewPatientEntry(string patientId, CommunicationPreferences communicationPreferences, string nhsNumber, string generalPractitionerId);
    }
}