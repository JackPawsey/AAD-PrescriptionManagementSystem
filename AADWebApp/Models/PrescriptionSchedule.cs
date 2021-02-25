using AADWebApp.Interfaces;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace AADWebApp.Models
{
    public class PrescriptionSchedule
    {
        public Prescription _Prescription { get; set; }

        private Timer Timer { get; set; }
        private int Occurances { get; set; }
        private double Interval { get; set; }
        private INotificationService _notificationService;
        
        public PrescriptionSchedule(Prescription Prescription, INotificationService notificationService)
        {
            _Prescription = Prescription;
            _notificationService = notificationService;

            SetupTimer();
        }

        private void SetupTimer()
        {
            TimeSpan prescriptionDuration = _Prescription.DateEnd - _Prescription.DateStart;

            if (_Prescription.IssueFrequency.ToString().Equals("Minutely"))
            {
                Interval = TimeSpan.FromMinutes(1).TotalMilliseconds;
            }
            else if (_Prescription.IssueFrequency.ToString().Equals("Weekly"))
            {
                Interval = TimeSpan.FromDays(7).TotalMilliseconds;
            }
            else if (_Prescription.IssueFrequency.ToString().Equals("BiWeekly"))
            {
                Interval = TimeSpan.FromDays(14).TotalMilliseconds;
            }
            else if (_Prescription.IssueFrequency.ToString().Equals("Monthly"))
            {
                Interval = TimeSpan.FromDays(28).TotalMilliseconds; // Could use nested if to determine days in month
            }
            else // Assume BiMonthly
            {
                Interval = TimeSpan.FromDays(56).TotalMilliseconds; // Could use nested if to determine days in month
            }

            Occurances = (int)(prescriptionDuration.TotalMilliseconds / Interval);

            CreateTimer();
        }

        private void CreateTimer()
        {
            if (Occurances > 0)
            {
                Occurances--;
                Timer = new Timer(Interval); //Timer duration in Milliseconds
                Timer.Elapsed += new ElapsedEventHandler(TimerElapsed);
                Timer.Start();
                Console.WriteLine("Timer " + _Prescription.Id + " started");
            }
        }

        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Timer.Stop();
            
            Console.WriteLine("Timer " + _Prescription.Id + " is doing stuff and has " + Occurances + " occurances remaining");
            await test();

            CreateTimer();
        }

        private async Task test()
        {
            await _notificationService.SendPrescriptionNotification(_Prescription, Occurances);
        }

        public void CancelSchedule()
        {
            Timer.Stop();
            Console.WriteLine("Timer " + _Prescription.Id + " stopped");
        }
    }
}
