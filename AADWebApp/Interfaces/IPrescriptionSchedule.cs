using AADWebApp.Models;
using System.Threading.Tasks;

namespace AADWebApp.Interfaces
{
    public interface IPrescriptionSchedule
    {
        public Prescription Prescription { get; set; }
        public Task SetupTimerAsync();
        public void CancelSchedule();
    }
}