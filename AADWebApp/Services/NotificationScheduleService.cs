using System;
using System.Collections.Generic;
using System.Linq;
using AADWebApp.Interfaces;
using AADWebApp.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AADWebApp.Services
{
    public class NotificationScheduleService : INotificationScheduleService
    {
        private readonly List<IPrescriptionSchedule> _prescriptionSchedules = new List<IPrescriptionSchedule>();
        private readonly IServiceProvider _serviceProvider;

        public NotificationScheduleService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void CreatePrescriptionSchedule(Prescription prescription)
        {
            var prescriptionSchedule = _serviceProvider.GetService<IPrescriptionSchedule>();
            prescriptionSchedule.Prescription = prescription;
            prescriptionSchedule.SetupTimer();

            _prescriptionSchedules.Add(prescriptionSchedule);
        }

        public void CancelPrescriptionSchedule(int id)
        {
            foreach (var prescriptionSchedule in _prescriptionSchedules.Where(schedule => schedule.Prescription.Id == id))
            {
                prescriptionSchedule.CancelSchedule();
            }
        }
    }
}