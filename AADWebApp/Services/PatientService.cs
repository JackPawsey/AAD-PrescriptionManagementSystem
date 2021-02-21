using System;
using System.Collections.Generic;
using System.Linq;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using static AADWebApp.Services.DatabaseService;

namespace AADWebApp.Services
{
    public class PatientService : IPatientService
    {
        private readonly IDatabaseService _databaseService;

        public PatientService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public enum CommunicationPreferences
        {
            Email,
            Sms,
            Both
        }

        public IEnumerable<Patient> GetPatients(string? id = null)
        {
            var patients = new List<Patient>();

            _databaseService.ConnectToMssqlServer(AvailableDatabases.program_data);

            using var result = _databaseService.RetrieveTable("patients", "id", id);

            while (result.Read())
            {
                patients.Add(new Patient
                {
                    Id = (string) result.GetValue(0),
                    CommunicationPreferences = (CommunicationPreferences) Enum.Parse(typeof(CommunicationPreferences), result.GetValue(1).ToString() ?? throw new InvalidOperationException()),
                    NhsNumber = (string) result.GetValue(2),
                    GeneralPractitioner = (string) result.GetValue(3)
                });
            }

            return patients.AsEnumerable();
        }

        public int SetCommunicationPreferences(string patientId, CommunicationPreferences communicationPreferences)
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.program_data);

            return _databaseService.ExecuteNonQuery($"UPDATE patients SET comm_preferences = '{(short) communicationPreferences}' WHERE id = '{patientId}'");
        }

        public int UpdateGeneralPractitioner(string patientId, string generalPractitionerId)
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.program_data);

            return _databaseService.ExecuteNonQuery($"UPDATE patients SET general_practitioner = '{generalPractitionerId}' WHERE id = '{patientId}'");
        }

        public int CreateNewPatientEntry(string patientId, CommunicationPreferences communicationPreferences, string nhsNumber, string generalPractitionerId)
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.program_data);

            return _databaseService.ExecuteNonQuery($"INSERT INTO patients (id, comm_preferences, nhs_number, general_practitioner) VALUES ('{patientId}', '{(short) communicationPreferences}', '{nhsNumber}', '{generalPractitionerId}')");
        }
    }
}