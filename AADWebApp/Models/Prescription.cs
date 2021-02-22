using System;
using static AADWebApp.Services.PrescriptionService;

namespace AADWebApp.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public int MedicationId { get; set; }
        public string PatientId { get; set; }
        public int Dosage { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public PrescriptionStatus PrescriptionStatus { get; set; }
        public string IssueFrequency { get; set; }
    }
}