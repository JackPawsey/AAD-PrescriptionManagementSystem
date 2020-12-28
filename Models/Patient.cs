using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        public Account Account { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String Age { get; set; }

        public IEnumerable<BloodTestResult> BloodTestResults { get; set; }
    }
}
