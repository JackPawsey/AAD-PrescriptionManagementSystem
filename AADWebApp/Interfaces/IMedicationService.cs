using System.Collections.Generic;
using AADWebApp.Models;

namespace AADWebApp.Interfaces
{
    public interface IMedicationService
    {
        public IEnumerable<Medication> GetMedications(short? id = null);
    }
}