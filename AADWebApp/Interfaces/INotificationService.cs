using AADWebApp.Models;
using System;
using System.Threading.Tasks;

namespace AADWebApp.Interfaces
{
    public interface INotificationService
    {
        public Task<bool> SendPrescriptionNotification(Prescription prescription, int occurrences, DateTime nextCollectionTime);
        public Task<bool> SendCollectionTimeNotification(Prescription prescription, DateTime collectionTime);
        public Task<bool> SendCancellationNotification(Prescription prescription, DateTime cancellationTime);
        public Task<bool> SendBloodTestRequestNotification(Prescription prescription, BloodTest bloodTest, DateTime requestTime);
        public Task<bool> SendBloodTestTimeUpdateNotification(Prescription prescription, BloodTestRequest bloodTestRequest, DateTime newTime);
    }
}
