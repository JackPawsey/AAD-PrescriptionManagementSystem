namespace AADWebApp.Models
{
    public class Medication
    {
        public int Id { get; set; }
        public string MedicationName { get; set; }
        public int? BloodWorkRestrictionLevel { get; set; }
    }
}