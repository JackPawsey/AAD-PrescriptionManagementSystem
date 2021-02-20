using System.Collections.Generic;

namespace AADWebApp.Models
{
    public class Patient
    {
        public int id { get; set; }
        public IEnumerable<Prescription> prescriptions { get; set; }
    }
}