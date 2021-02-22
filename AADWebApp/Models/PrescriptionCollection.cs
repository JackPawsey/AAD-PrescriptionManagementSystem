using System;
using static AADWebApp.Services.PrescriptionCollectionService;

namespace AADWebApp.Models
{
    public class PrescriptionCollection
    {
        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public CollectionStatus CollectionStatus { get; set; }
        public DateTime CollectionStatusUpdated { get; set; }
        public DateTime CollectionTime { get; set; }
    }
}
