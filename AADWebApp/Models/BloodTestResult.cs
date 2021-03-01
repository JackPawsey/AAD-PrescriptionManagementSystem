using System;

namespace AADWebApp.Models
{
    public class BloodTestResult
    {
        public short Id { get; set; }
        public short BloodTestRequestId { get; set; }
        public bool Result { get; set; }
        public DateTime ResultTime { get; set; }
    }
}