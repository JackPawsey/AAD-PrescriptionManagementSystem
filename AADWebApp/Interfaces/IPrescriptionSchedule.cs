using AADWebApp.Models;

namespace AADWebApp.Interfaces
{
    public interface IPrescriptionSchedule
    {
        public Prescription Prescription { get; set; }
        public void SetupTimer();
        public void CancelSchedule();
    }
}