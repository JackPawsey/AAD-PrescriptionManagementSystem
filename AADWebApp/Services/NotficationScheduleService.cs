using AADWebApp.Interfaces;
using AADWebApp.Models;
using System.Collections.Generic;

namespace AADWebApp.Services
{
    public class NotficationScheduleService : INotificationScheduleService
    {
        private List<PrescriptionSchedule> PrescriptionSchedules = new List<PrescriptionSchedule>();
        private readonly INotificationService _notifcationService;

        public NotficationScheduleService(INotificationService notifcationService)
        {
            _notifcationService = notifcationService;
        }

        public void CreatePrescriptionSchedule(Prescription Prescription)
        {
            PrescriptionSchedule PrescriptionSchedule = new PrescriptionSchedule(Prescription, _notifcationService);

            PrescriptionSchedules.Add(PrescriptionSchedule);
        }

        public void CancelPrescriptionSchedule(int id)
        {
            for (int x = 0; x < PrescriptionSchedules.Count; x++)
            {
                if (PrescriptionSchedules[x]._Prescription.Id == id)
                {
                    PrescriptionSchedules[x].CancelSchedule();
                }
            }
        }
    }
}
