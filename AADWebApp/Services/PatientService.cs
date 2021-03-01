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

        public IEnumerable<Patient> GetPatients(string? patientId = null)
        {
            var patients = new List<Patient>();

            _databaseService.ConnectToMssqlServer(AvailableDatabases.ProgramData);

            using var result = _databaseService.RetrieveTable("Patients", "Id", patientId);

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
            _databaseService.ConnectToMssqlServer(AvailableDatabases.ProgramData);

            return _databaseService.ExecuteNonQuery($"UPDATE Patients SET CommunicationPreferences = '{communicationPreferences}' WHERE Id = '{patientId}'");
        }

        public int UpdateGeneralPractitioner(string patientId, string generalPractitionerName)
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.ProgramData);

            return _databaseService.ExecuteNonQuery($"UPDATE Patients SET GeneralPractitioner = '{generalPractitionerName}' WHERE Id = '{patientId}'");
        }

        public int CreateNewPatientEntry(string patientId, CommunicationPreferences communicationPreferences, string nhsNumber, string generalPractitionerName)
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.ProgramData);

            return _databaseService.ExecuteNonQuery($"INSERT INTO Patients (Id, CommunicationPreferences, NhsNumber, GeneralPractitioner) VALUES ('{patientId}', '{communicationPreferences}', '{nhsNumber}', '{generalPractitionerName}')");
        }

        public int DeletePatient(string patientId)
        {
            _databaseService.ConnectToMssqlServer(AvailableDatabases.ProgramData);

            return _databaseService.ExecuteNonQuery($"DELETE FROM Patients WHERE Id = '{patientId}'");
        }
    }
}