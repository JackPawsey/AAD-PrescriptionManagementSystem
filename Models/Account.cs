using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApp.Models
{
    public class BloodTest
    {
        public int BloodTestId { get; set; }

        public int PatientId { get; set; }

        public String BloodTestType { get; set; }

        public DateTime DateRequested { get; set; }
    }
}
