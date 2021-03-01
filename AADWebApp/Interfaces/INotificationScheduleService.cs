using AADWebApp.Models;
using System.Collections.Generic;

namespace AADWebApp.Interfaces
{
    public interface INotificationScheduleService
    {
        public IEnumerable<IPrescriptionSchedule> GetPrescriptionSchedules();
        public bool CreatePrescriptionSchedule(Prescription prescription);
        public bool CancelPrescriptionSchedule(short id);
    }
}
