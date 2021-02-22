using AADWebApp.Models;
using System;
using System.Collections.Generic;
using static AADWebApp.Services.PrescriptionCollectionService;

namespace AADWebApp.Interfaces
{
    public interface IPrescriptionCollectionService
    {
        public IEnumerable<PrescriptionCollection> GetPrescriptionCollections(short? id = null);
        public int CreatePrescriptionCollection(int PrescriptionId, CollectionStatus CollectionStatus, DateTime CollectionStatusUpdated, DateTime CollectionTime);
        public int SetPrescriptionCollectionStatus(int Id, CollectionStatus CollectionStatus);
        public int SetPrescriptionCollectionTime(int Id, DateTime CollectionTime);
    }
}
