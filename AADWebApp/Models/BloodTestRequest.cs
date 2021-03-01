using System;
using static AADWebApp.Services.BloodTestService;

namespace AADWebApp.Models
{
    public class BloodTestRequest
    {
        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public int BloodTestId { get; set; }
        public DateTime AppointmentTime { get; set; }
        public BloodTestRequestStatus BloodTestStatus { get; set; }
    }
}