using System;
using static AADWebApp.Services.PrescriptionCollectionService;

namespace AADWebApp.Models
{
    public class PrescriptionCollection
    {
        public short Id { get; set; }
        public short PrescriptionId { get; set; }
        public CollectionStatus CollectionStatus { get; set; }
        public DateTime CollectionStatusUpdated { get; set; }
        public DateTime CollectionTime { get; set; }
    }
}
