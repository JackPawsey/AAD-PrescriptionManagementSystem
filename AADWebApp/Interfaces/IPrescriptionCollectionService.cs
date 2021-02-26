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
        public int CreatePrescriptionCollection(int prescriptionId, CollectionStatus collectionStatus, DateTime collectionTime);
        public int SetPrescriptionCollectionStatus(int id, CollectionStatus collectionStatus);
        public Task<int> SetPrescriptionCollectionTimeAsync(int id, DateTime collectionTime);
    }
}