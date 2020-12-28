using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApp.Models
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }

        public int PatientId { get; set; }

        public DateTime PrescriptionEnd { get; set; }

        public String MedicationType { get; set; }

        public int Dosage { get; set; }

        public String PrescriptionStatus { get; set; }

        public DateTime CollectionDateTime { get; set; }
    }
}
