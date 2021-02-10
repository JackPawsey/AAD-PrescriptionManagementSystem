using System;

namespace AADWebApp.Models
{
    public class BloodTestResult
    {
        public DateTime Occurance { get; set; }

        public string BloodTestType { get; set; }

        public int Result { get; set; }
    }
}