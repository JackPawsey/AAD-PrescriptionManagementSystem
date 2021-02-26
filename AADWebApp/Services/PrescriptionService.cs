using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public enum IssueFrequency
        {
            Minutely, // FOR DEMONSTRATION PURPOSES ONLY
            Weekly,
            BiWeekly,
            Monthly,
            BiMonthly
        }

        private readonly IDatabaseService _databaseService;
        private readonly INotificationScheduleService _notificationScheduleService;

        public PrescriptionService(IDatabaseService databaseService, INotificationScheduleService notificationScheduleService)
        {
            _databaseService = databaseService;
            _notificationScheduleService = notificationScheduleService;
        }

        public IEnumerable<Prescription> GetPrescriptions(short? id = null)
        {
            var prescriptions = new List<Prescription>();

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            //GET prescriptions TABLE
            using var result = _databaseService.RetrieveTable("Prescriptions", "Id", id);

            while (result.Read())
            {
                prescriptions.Add(new Prescription
                {
                    Id = (short) result.GetValue(0),
                    MedicationId = (short) result.GetValue(1),
                    PatientId = (string) result.GetValue(2),
                    Dosage = (short) result.GetValue(3),
                    DateStart = (DateTime) result.GetValue(4),
                    DateEnd = (DateTime) result.GetValue(5),
                    PrescriptionStatus = (PrescriptionStatus) Enum.Parse(typeof(PrescriptionStatus), result.GetValue(6).ToString() ?? throw new InvalidOperationException()),
                    IssueFrequency = (IssueFrequency) Enum.Parse(typeof(IssueFrequency), result.GetValue(7).ToString() ?? throw new InvalidOperationException())
                });
            }

            return prescriptions.AsEnumerable();
        }

        public IEnumerable<Prescription> GetPrescriptionsByPatientId(string? id = null)
        {
            var prescriptions = new List<Prescription>();

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            //GET prescriptions TABLE
            using var result = _databaseService.RetrieveTable("Prescriptions", "PatientId", id);

            while (result.Read())
            {
                prescriptions.Add(new Prescription
                {
                    Id = (short) result.GetValue(0),
                    MedicationId = (short) result.GetValue(1),
                    PatientId = (string) result.GetValue(2),
                    Dosage = (short) result.GetValue(3),
                    DateStart = (DateTime) result.GetValue(4),
                    DateEnd = (DateTime) result.GetValue(5),
                    PrescriptionStatus = (PrescriptionStatus) Enum.Parse(typeof(PrescriptionStatus), result.GetValue(6).ToString() ?? throw new InvalidOperationException()),
                    IssueFrequency = (IssueFrequency) Enum.Parse(typeof(IssueFrequency), result.GetValue(7).ToString() ?? throw new InvalidOperationException())
                });
            }

            return prescriptions.AsEnumerable();
        }

        public int CreatePrescription(int medicationId, string patientId, int dosage, DateTime dateStart, DateTime dateEnd, PrescriptionStatus prescriptionStatus, IssueFrequency issueFrequency)
        {
            if ((dateStart < dateEnd) && (prescriptionStatus != PrescriptionStatus.Approved)) // CHECK THAT dateEnd IS AFTER dateStart
            {
                _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

                //CREATE prescriptions TABLE ROW
                return _databaseService.ExecuteNonQuery($"INSERT INTO Prescriptions (MedicationId, PatientId, Dosage, DateStart, DateEnd, PrescriptionStatus, IssueFrequency) VALUES ('{medicationId}', '{patientId}', '{dosage}', '{dateStart:yyyy-MM-dd HH:mm:ss}', '{dateEnd:yyyy-MM-dd HH:mm:ss}', '{prescriptionStatus}', '{issueFrequency}')");
            }
            else
            {
                return 0;
            }
        }

        public int CancelPrescription(int id)
        {
            if (GetPrescriptions((short?) id).ElementAt(0).PrescriptionStatus.ToString().Equals("Approved"))
            {
                _notificationScheduleService.CancelPrescriptionSchedule(id); // Cancel PrescriptionSchedule when prescription is cancelled and its has been approved
            }

            return SetPrescriptionStatus(id, PrescriptionStatus.Terminated); // Otherwise it does not have a PrescriptionSchedule
        }

        public int SetPrescriptionStatus(int id, PrescriptionStatus prescriptionStatus) // Setting an already approved prescription to 'Approved' will restart is presriptionSchedule!
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            if (prescriptionStatus.ToString().Equals("Approved"))
            {
                var prescription = GetPrescriptions((short?) id);

                _notificationScheduleService.CreatePrescriptionSchedule(prescription.ElementAt(0)); // Start PrescriptionSchedule when prescription is approved
            }

            //UPDATE prescriptions TABLE ROW prescription_status COLUMN
            return _databaseService.ExecuteNonQuery($"UPDATE Prescriptions SET PrescriptionStatus = '{prescriptionStatus}' WHERE Id = '{id}'");
        }
    }
}