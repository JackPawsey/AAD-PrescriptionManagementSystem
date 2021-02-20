using System;

namespace AADWebApp.Models
{
    public class BloodTestResult
    {
        public int id { get; set; }
        public int blood_test_id { get; set; }
        public bool result { get; set; }
        public DateTime resultTime { get; set; }
    }
}