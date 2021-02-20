using System;

namespace AADWebApp.Models
{
    public class BloodTestRequest
    {
        public int id { get; set; }

        public int prescription_id { get; set; }

        public int blood_test_id { get; set; }

        public DateTime appointment_time { get; set; }
    }
}
