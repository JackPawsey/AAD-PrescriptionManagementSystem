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
            BloodworkReceived,
            Approved,
            Finished,
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
        private readonly INotificationService _notificationService;
        private readonly IPrescriptionCollectionService _prescriptionCollectionService;
        private readonly IBloodTestService _bloodTestService;

        public PrescriptionService(IDatabaseService databaseService, INotificationScheduleService notificationScheduleService, INotificationService notificationService, IPrescriptionCollectionService prescriptionCollectionService, IBloodTestService bloodTestService)
        {
            _databaseService = databaseService;
            _notificationScheduleService = notificationScheduleService;
            _notificationService = notificationService;
            _prescriptionCollectionService = prescriptionCollectionService;
            _bloodTestService = bloodTestService;
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

        public int CreatePrescription(short medicationId, string patientId, int dosage, DateTime dateStart, DateTime dateEnd, IssueFrequency issueFrequency)
        {
            if (dateStart < dateEnd) // CHECK THAT dateEnd IS AFTER dateStart
            {
                _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

                //CREATE prescriptions TABLE ROW
                var dbResult = _databaseService.ExecuteNonQuery($"INSERT INTO Prescriptions (MedicationId, PatientId, Dosage, DateStart, DateEnd, PrescriptionStatus, IssueFrequency) VALUES ('{medicationId}', '{patientId}', '{dosage}', '{dateStart:yyyy-MM-dd HH:mm:ss}', '{dateEnd:yyyy-MM-dd HH:mm:ss}', '{PrescriptionStatus.PendingApproval}', '{issueFrequency}')");

                _databaseService.CloseConnection();

                return dbResult;
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> CancelPrescriptionAsync(short id)
        {
            var prescription = GetPrescriptions(id).ElementAt(0);

            if (prescription.PrescriptionStatus.ToString().Equals("Approved"))
            {
                _notificationScheduleService.CancelPrescriptionSchedule(id); // Cancel PrescriptionSchedule when prescription is cancelled and its has been approved
            }

            var prescriptions = _prescriptionCollectionService.GetPrescriptionCollectionsByPrescriptionId(id);

            foreach (var item in prescriptions)
            {
                _prescriptionCollectionService.CancelPrescriptionCollection(item.Id); // Set any PrescriptionCollections to Cancelled
            }

            var bloodTestRequests = _bloodTestService.GetBloodTestRequests(id);

            foreach (var bloodTestRequest in bloodTestRequests)
            {
                _bloodTestService.CancelBloodTestRequest(bloodTestRequest.Id); // Cancel bloodTestRequests for this prescription
            }

            await _notificationService.SendCancellationNotification(prescription, DateTime.Now); // Send notification to patient

            return SetPrescriptionStatus(id, PrescriptionStatus.Terminated);
        }

        public int SetPrescriptionStatus(short id, PrescriptionStatus prescriptionStatus)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            var prescription = GetPrescriptions(id).ElementAt(0);

            if (prescription.PrescriptionStatus.Equals(PrescriptionStatus.Approved))
            {
                //UPDATE prescriptions TABLE ROW prescription_status COLUMN
                var dbResult1 = _databaseService.ExecuteNonQuery($"UPDATE Prescriptions SET PrescriptionStatus = '{prescriptionStatus}' WHERE Id = '{id}'");

                _databaseService.CloseConnection();

                return dbResult1;
            }

            if (prescriptionStatus.Equals(PrescriptionStatus.Approved))
            {
                _notificationScheduleService.CreatePrescriptionSchedule(prescription); // Start PrescriptionSchedule when prescription is approved
            }

            //UPDATE prescriptions TABLE ROW prescription_status COLUMN
            var dbResult2 = _databaseService.ExecuteNonQuery($"UPDATE Prescriptions SET PrescriptionStatus = '{prescriptionStatus}' WHERE Id = '{id}'");

            _databaseService.CloseConnection();

            return dbResult2;
        }
    }
}