namespace AADWebApp.Models
{
    public class Medication
    {
        public int id { get; set; }
        public string medication { get; set; }
        public int? blood_work_restriction_level { get; set; }
    }
}
