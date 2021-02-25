using AADWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AADWebApp.Services.PrescriptionService;

namespace AADWebApp.Interfaces
{
    public interface INotificationScheduleService
    {
        //public void CreatePrescriptionSchedule(int id, IssueFrequency IssueFrequency, DateTime DateStart, DateTime DateEnd);
        public void CreatePrescriptionSchedule(Prescription Prescription);
        public void CancelPrescriptionSchedule(int id);
    }
}
