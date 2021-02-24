using System;
using System.Collections.Generic;
using System.Linq;
using AADWebApp.Interfaces;
using AADWebApp.Models;

namespace AADWebApp.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly IDatabaseService _databaseService;

        public MedicationService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<Medication> GetMedications(short? id = null)
        {
            var medications = new List<Medication>();

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.ProgramData);

            //GET Medications TABLE
            using var result = _databaseService.RetrieveTable("Medications", "Id", id);

            while (result.Read())
            {
                medications.Add(new Medication
                {
                    Id = (short) result.GetValue(0),
                    MedicationName = (string) result.GetValue(1),
                    BloodWorkRestrictionLevel = Convert.IsDBNull(result.GetValue(2)) ? (short?) null : (short) result.GetValue(2)
                });
            }

            return medications.AsEnumerable();
        }
    }
}