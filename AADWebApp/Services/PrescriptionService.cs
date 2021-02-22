using System;
using System.Collections.Generic;
using System.Linq;
using AADWebApp.Interfaces;
using AADWebApp.Models;

namespace AADWebApp.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        public enum PrescriptionStatus
        {
            PendingApproval,
            AwaitingBloodWork,
            Approved,
            Declined,
            Terminated
        }

        private readonly IDatabaseService _databaseService;

        public PrescriptionService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<Prescription> GetPrescriptions(short? id = null)
        {
            var prescriptions = new List<Prescription>();

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //GET prescriptions TABLE
            using var result = _databaseService.RetrieveTable("prescriptions", "id", id);

            while (result.Read())
            {
                prescriptions.Add(new Prescription
                {
                    Id = (short)result.GetValue(0),
                    MedicationId = (short)result.GetValue(1),
                    PatientId = (string)result.GetValue(2),
                    Dosage = (short)result.GetValue(3),
                    DateStart = (DateTime)result.GetValue(4),
                    DateEnd = (DateTime)result.GetValue(5),
                    PrescriptionStatus = (string)result.GetValue(6),
                    IssueFrequency = (string)result.GetValue(7)
                });
            }

            return prescriptions.AsEnumerable();
        }

        public int CreatePrescription(int MedicationId, string PatientId, int Dosage, DateTime DateStart, DateTime DateEnd, PrescriptionStatus PrescriptionStatus, string IssueFrequency)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //CREATE prescriptions TABLE ROW
            return _databaseService.ExecuteNonQuery($"INSERT INTO prescriptions (medication_id, patient_id, dosage, date_start, date_end, prescription_status, issue_frequency) VALUES ('{MedicationId}', '{PatientId}', '{Dosage}', '{DateStart:yyyy-MM-dd HH:mm:ss}', '{DateEnd:yyyy-MM-dd HH:mm:ss}', '{PrescriptionStatus}', '{IssueFrequency}')");
        }

        public int CancelPrescription(int Id)
        {
            return SetPrescriptionStatus(Id, PrescriptionStatus.Terminated);
        }

        public int SetPrescriptionStatus(int Id, PrescriptionStatus PrescriptionStatus)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //UPDATE prescriptions TABLE ROW prescription_status COLUMN
            return _databaseService.ExecuteNonQuery($"UPDATE prescriptions SET prescription_status = '{PrescriptionStatus}' WHERE id = '{Id}'");
        }
    }
}