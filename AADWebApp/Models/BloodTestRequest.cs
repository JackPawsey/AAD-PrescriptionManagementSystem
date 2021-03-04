using System;
using static AADWebApp.Services.BloodTestService;

namespace AADWebApp.Models
{
    public class BloodTestRequest
    {
        public short Id { get; set; }
        public short PrescriptionId { get; set; }
        public short BloodTestId { get; set; }
        public DateTime? AppointmentTime { get; set; }
        public BloodTestRequestStatus BloodTestStatus { get; set; }
    }
}