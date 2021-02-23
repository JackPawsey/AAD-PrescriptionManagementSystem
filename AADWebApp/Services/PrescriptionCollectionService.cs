using System;
using System.Collections.Generic;
using System.Linq;
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

        public PrescriptionCollectionService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<PrescriptionCollection> GetPrescriptionCollections(short? id = null)
        {
            var prescriptionCollections = new List<PrescriptionCollection>();

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //GET prescription_collections TABLE
            using var result = _databaseService.RetrieveTable("prescription_collections", "id", id);

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

        public int CreatePrescriptionCollection(int prescriptionId, CollectionStatus collectionStatus, DateTime collectionStatusUpdated, DateTime collectionTime)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //CREATE prescription_collections TABLE ROW
            return _databaseService.ExecuteNonQuery($"INSERT INTO prescription_collections (prescription_id, collection_status, collection_status_updated, collection_time) VALUES ('{prescriptionId}', '{collectionStatus}', '{collectionStatusUpdated:yyyy-MM-dd HH:mm:ss}', '{collectionTime:yyyy-MM-dd HH:mm:ss}')");
        }

        public int SetPrescriptionCollectionStatus(int id, CollectionStatus collectionStatus)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //UPDATE prescription_collections TABLE ROW collection_status
            return _databaseService.ExecuteNonQuery($"UPDATE prescription_collections SET collection_status = '{collectionStatus}', collection_status_updated = '{DateTime.Now:yyyy-MM-dd HH:mm:ss}' WHERE id = '{id}'");
        }

        public int SetPrescriptionCollectionTime(int id, DateTime collectionTime)
        {
            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //UPDATE prescription_collections TABLE ROW collection_time
            return _databaseService.ExecuteNonQuery($"UPDATE prescription_collections SET collection_time = '{collectionTime:yyyy-MM-dd HH:mm:ss}' WHERE id = '{id}'");
        }
    }
}