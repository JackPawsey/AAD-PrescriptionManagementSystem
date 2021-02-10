using System;

namespace AADWebApp.Models
{
    public class BloodTest
    {
        public int BloodTestId { get; set; }

        public int PatientId { get; set; }

        public string BloodTestType { get; set; }

        public DateTime DateRequested { get; set; }
    }
}