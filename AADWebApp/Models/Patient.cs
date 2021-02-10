using System.Collections.Generic;

namespace AADWebApp.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        public Account Account { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Age { get; set; }

        public IEnumerable<BloodTestResult> BloodTestResults { get; set; }
    }
}