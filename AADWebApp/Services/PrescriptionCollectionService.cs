using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AADWebApp.Interfaces;
using AADWebApp.Models;

namespace AADWebApp.Services
{
    public class PrescriptionCollectionService : IPrescriptionCollectionService
    {
        public enum CollectionStatus
        {
            BeingPrepared,
            CollectionReady,
            Collected
        }

        private readonly IDatabaseService _databaseService;
        private readonly IPrescriptionService _prescriptionService;
        private readonly INotificationService _notificationService;

        public PrescriptionCollectionService(IDatabaseService databaseService, IPrescriptionService prescriptionService, INotificationService notificationService)
        {
            _databaseService = databaseService;
            _prescriptionService = prescriptionService;
            _notificationService = notificationService;
        }

        public IEnumerable<PrescriptionCollection> GetPrescriptionCollections(short? id = null)
        {
            var prescriptionCollections = new List<PrescriptionCollection>();

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            //GET PrescriptionCollections TABLE
            using var result = _databaseService.RetrieveTable("PrescriptionCollections", "Id", id);

            while (result.Read())
            {
                prescriptionCollections.Add(new PrescriptionCollection
                {
                    Id = (short) result.GetValue(0),
                    PrescriptionId = (short) result.GetValue(1),
                    CollectionStatus = (CollectionStatus) Enum.Parse(typeof(CollectionStatus), result.GetValue(2).ToString() ?? throw new InvalidOperationException()),
                    CollectionStatusUpdated = (DateTime) result.GetValue(3),
                    CollectionTime = (DateTime) result.GetValue(4)
                });
            }

            return prescriptionCollections.AsEnumerable();
        }

        public IEnumerable<PrescriptionCollection> GetPrescriptionCollectionsByPrescriptionId(short? id = null)
        {
            var prescriptionCollections = new List<PrescriptionCollection>();

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            //GET PrescriptionCollections TABLE
            using var result = _databaseService.RetrieveTable("PrescriptionCollections", "PrescriptionId", id);

            while (result.Read())
            {
                prescriptionCollections.Add(new PrescriptionCollection
                {
                    Id = (short)result.GetValue(0),
                    PrescriptionId = (short)result.GetValue(1),
                    CollectionStatus = (CollectionStatus)Enum.Parse(typeof(CollectionStatus), result.GetValue(2).ToString() ?? throw new InvalidOperationException()),
                    CollectionStatusUpdated = (DateTime)result.GetValue(3),
                    CollectionTime = (DateTime)result.GetValue(4)
                });
            }

            return prescriptionCollections.AsEnumerable();
        }

        public int CreatePrescriptionCollection(int prescriptionId, CollectionStatus collectionStatus, DateTime collectionTime)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            //CREATE PrescriptionCollections TABLE ROW
            return _databaseService.ExecuteNonQuery($"INSERT INTO PrescriptionCollections (PrescriptionId, CollectionStatus, CollectionStatusUpdated, CollectionTime) VALUES ('{prescriptionId}', '{collectionStatus}', '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', '{collectionTime:yyyy-MM-dd HH:mm:ss}')");
        }

        public int SetPrescriptionCollectionStatus(int id, CollectionStatus collectionStatus)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            //UPDATE PrescriptionCollections TABLE ROW CollectionStatus
            return _databaseService.ExecuteNonQuery($"UPDATE PrescriptionCollections SET CollectionStatus = '{collectionStatus}', CollectionStatusUpdated = '{DateTime.Now:yyyy-MM-dd HH:mm:ss}' WHERE Id = '{id}'");
        }

        public async Task<int> SetPrescriptionCollectionTimeAsync(PrescriptionCollection prescriptionCollection, DateTime collectionTime)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            var prescription = _prescriptionService.GetPrescriptions((short?) prescriptionCollection.PrescriptionId).ElementAt(0);

            if (collectionTime > prescription.DateStart && collectionTime < prescription.DateEnd)
            {
                await _notificationService.SendCollectionTimeNotification(prescription, collectionTime);

                //UPDATE prescription_collections TABLE ROW collection_time
                return _databaseService.ExecuteNonQuery($"UPDATE PrescriptionCollections SET CollectionTime = '{collectionTime:yyyy-MM-dd HH:mm:ss}' WHERE Id = '{prescriptionCollection.Id}'");
            }

            return 0;
        }
    }
}