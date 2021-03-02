using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AADWebApp.Models;
using static AADWebApp.Services.PrescriptionCollectionService;

namespace AADWebApp.Interfaces
{
    public interface IPrescriptionCollectionService
    {
        public IEnumerable<PrescriptionCollection> GetPrescriptionCollections(short? id = null);
        public IEnumerable<PrescriptionCollection> GetPrescriptionCollectionsByPrescriptionId(short? id = null);
        public int CreatePrescriptionCollection(short prescriptionId, CollectionStatus collectionStatus, DateTime collectionTime);
        public int CancelPrescriptionCollection(short id);
        public int SetPrescriptionCollectionStatus(short id, CollectionStatus collectionStatus);
        public int SetPrescriptionCollectionCollected(short id);
        public Task<int> SetPrescriptionCollectionTimeAsync(Prescription prescription, DateTime collectionTime);
    }
}