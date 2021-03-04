using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using AADWebApp.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using static AADWebApp.Services.PrescriptionCollectionService;
using static AADWebApp.Services.PrescriptionService.IssueFrequency;

namespace AADWebApp.Models
{
    public class PrescriptionSchedule : IPrescriptionSchedule
    {
        public Prescription Prescription { get; set; }

        private Timer Timer { get; set; }
        private int Occurrences { get; set; }
        private double Interval { get; set; }

        private readonly IServiceProvider _serviceProvider;

        public PrescriptionSchedule(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SetupTimerAsync()
        {
            var prescriptionDuration = Prescription.DateEnd - Prescription.DateStart;

            Interval = Prescription.IssueFrequency switch
            {
                Minutely => TimeSpan.FromMinutes(1).TotalMilliseconds,
                Weekly => TimeSpan.FromDays(7).TotalMilliseconds,
                BiWeekly => TimeSpan.FromDays(14).TotalMilliseconds,
                Monthly => TimeSpan.FromDays(30).TotalMilliseconds, // Average number of days per month
                _ => TimeSpan.FromDays(60).TotalMilliseconds
            };

            Occurrences = (int) (prescriptionDuration.TotalMilliseconds / Interval);

            Occurrences--;

            var nextCollectionTime = DateTime.Now.AddMilliseconds(Interval);
            var prescriptionCollectionService = _serviceProvider.CreateScope().ServiceProvider.GetService<IPrescriptionCollectionService>();
            prescriptionCollectionService.CreatePrescriptionCollection(Prescription.Id, CollectionStatus.Pending, nextCollectionTime); // Send initial PrescriptionCollection

            var notificationService = _serviceProvider.CreateScope().ServiceProvider.GetService<INotificationService>();
            await notificationService.SendPrescriptionNotification(Prescription, Occurrences, nextCollectionTime); // Send initial Prescription email

            CreateTimer();
        }

        private void CreateTimer()
        {
            if (Occurrences > 0)
            {
                Occurrences--;
                Timer = new Timer(Interval); //Timer duration in Milliseconds
                Timer.Elapsed += async delegate { await TimerElapsed(Prescription, Occurrences, Interval); };
                Timer.Start();
                Console.WriteLine("Schedule for prescription " + Prescription.Id + " started");
            }
        }

        private async Task TimerElapsed(Prescription prescription, int occurrences, double interval)
        {
            Timer.Stop();
            Timer.Dispose();

            Console.WriteLine("Prescription " + prescription.Id + " interval has been reached. " + occurrences + " occurrences remaining");

            var nextCollectionTime = DateTime.Now.AddMilliseconds(interval);
            var prescriptionCollectionService = _serviceProvider.CreateScope().ServiceProvider.GetService<IPrescriptionCollectionService>();
            prescriptionCollectionService.CreatePrescriptionCollection(prescription.Id, CollectionStatus.Pending, nextCollectionTime);

            var notificationService = _serviceProvider.CreateScope().ServiceProvider.GetService<INotificationService>();
            await notificationService.SendPrescriptionNotification(prescription, occurrences, nextCollectionTime);

            CreateTimer();
        }

        public void CancelSchedule()
        {
            Timer.Stop();
            Console.WriteLine("Prescription " + Prescription.Id + " schedule stopped");
        }
    }
}