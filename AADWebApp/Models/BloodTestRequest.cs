using System;

namespace AADWebApp.Models
{
    public class BloodTestRequest
    {
        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public int BloodTestId { get; set; }
        public DateTime AppointmentTime { get; set; }
    }
}