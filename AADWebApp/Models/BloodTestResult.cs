using System;

namespace AADWebApp.Models
{
    public class BloodTestResult
    {
        public int Id { get; set; }
        public int BloodTestRequestId { get; set; }
        public bool Result { get; set; }
        public DateTime ResultTime { get; set; }
    }
}