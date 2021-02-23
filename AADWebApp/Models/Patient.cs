using static AADWebApp.Services.PatientService;

namespace AADWebApp.Models
{
    public class Patient
    {
        public string Id { get; set; }
        public CommunicationPreferences CommunicationPreferences { get; set; }
        public string NhsNumber { get; set; }
        public string GeneralPractitioner { get; set; }
    }
}