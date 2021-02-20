using AADWebApp.Models;
using System.Collections.Generic;

namespace AADWebApp.Interfaces
{
    public interface IMedicationService
    {
        public IEnumerable<Medication> GetMedications(short? id = null);
    }
}
