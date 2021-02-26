using System;
using System.Threading.Tasks;
using System.Timers;
using AADWebApp.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using static AADWebApp.Services.PrescriptionService.IssueFrequency;

namespace AADWebApp.Models
{
    public class PrescriptionSchedule : IPrescriptionSchedule
    {
        public Prescription Prescription { get; set; }

        private Timer Timer { get; set; }
        private int Occurances { get; set; }
        private double Interval { get; set; }

        private readonly IServiceProvider _serviceProvider;

        public PrescriptionSchedule(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void SetupTimer()
        {
            var prescriptionDuration = Prescription.DateEnd - Prescription.DateStart;

            //Console.WriteLine("prescription days: " + prescriptionDuration.Days);

            Interval = Prescription.IssueFrequency switch
            {
                Minutely => TimeSpan.FromMinutes(1).TotalMilliseconds,
                Weekly => TimeSpan.FromDays(7).TotalMilliseconds,
                BiWeekly => TimeSpan.FromDays(14).TotalMilliseconds,
                Monthly => TimeSpan.FromDays(30).TotalMilliseconds, // Average number of days per month
                _ => TimeSpan.FromDays(60).TotalMilliseconds
            };

            Console.WriteLine("monthly interval: " + TimeSpan.FromDays(prescriptionDuration.Days).TotalMilliseconds);

            Occurances = (int) (prescriptionDuration.TotalMilliseconds / Interval);

            CreateTimer();
        }

        private void CreateTimer()
        {
            if (Occurances > 0)
            {
                Occurances--;
                Timer = new Timer(Interval); //Timer duration in Milliseconds
                Timer.Elapsed += async delegate { await TimerElapsed(Prescription, Occurances); };
                Timer.Start();
                Console.WriteLine("Schedule for prescription " + Prescription.Id + " started");
            }
        }

        private async Task TimerElapsed(Prescription prescription, int occurrences)
        {
            Timer.Stop();
            Timer.Dispose();

            Console.WriteLine("Prescription " + prescription.Id + " interval has been reached. " + occurrences + " occurrences remaining");

            var notificationService = _serviceProvider.CreateScope().ServiceProvider.GetService<INotificationService>();

            await notificationService.SendPrescriptionNotification(prescription, occurrences);

            CreateTimer();
        }

        public void CancelSchedule()
        {
            Timer.Stop();
            Console.WriteLine("Prescription " + Prescription.Id + " schedule stopped");
        }
    }
}