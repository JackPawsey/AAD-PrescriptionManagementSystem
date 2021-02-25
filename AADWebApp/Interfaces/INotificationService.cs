using AADWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AADWebApp.Interfaces
{
    public interface INotificationService
    {
        public Task SendPrescriptionNotification(Prescription Prescription, int Occurances);
    }
}
