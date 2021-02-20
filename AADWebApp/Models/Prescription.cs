using System;

namespace AADWebApp.Models
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }
        public int PatientId { get; set; }
        public DateTime PrescriptionEnd { get; set; }
        public string MedicationType { get; set; }
        public int Dosage { get; set; }
        public string PrescriptionStatus { get; set; }
        public DateTime CollectionDateTime { get; set; }
    }
}