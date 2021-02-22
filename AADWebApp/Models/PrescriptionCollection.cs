using System;

namespace AADWebApp.Models
{
    public class PrescriptionCollection
    {
        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public string CollectionStatus { get; set; }
        public DateTime CollectionStatusUpdated { get; set; }
        public DateTime CollectionTime { get; set; }
    }
}
