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

        public IEnumerable<IPrescriptionSchedule> GetPrescriptionSchedules()
        {
            return _prescriptionSchedules.AsEnumerable();
        }

        public bool CreatePrescriptionSchedule(Prescription prescription)
        {
            try
            {
                var prescriptionSchedule = _serviceProvider.GetService<IPrescriptionSchedule>();
                prescriptionSchedule.Prescription = prescription;
                prescriptionSchedule.SetupTimer();

                _prescriptionSchedules.Add(prescriptionSchedule);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CancelPrescriptionSchedule(int id)
        {
            for (int i = 0; i < _prescriptionSchedules.Count(); i++)
            {
                if (_prescriptionSchedules[i].Prescription.Id == id)
                {
                    _prescriptionSchedules[i].CancelSchedule();

                    _prescriptionSchedules.RemoveAt(i);

                    return true;
                }
            }

            return false;
        }
    }
}