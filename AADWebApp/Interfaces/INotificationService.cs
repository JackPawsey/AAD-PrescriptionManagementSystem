using AADWebApp.Models;
using System;
using System.Threading.Tasks;

namespace AADWebApp.Interfaces
{
    public interface INotificationService
    {
        public Task SendPrescriptionNotification(Prescription Prescription, int Occurances, DateTime nextCollectionTime);
        public Task SendCollectionTimeNotification(Prescription prescription, DateTime collectionTime);
        public Task SendCancellationNotification(Prescription prescription, DateTime cancellationTime);
    }
}
