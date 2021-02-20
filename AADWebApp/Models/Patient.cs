using System.Collections.Generic;

namespace AADWebApp.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public IEnumerable<Prescription> Prescriptions { get; set; }
    }
}