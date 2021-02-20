using AADWebApp.Interfaces;
using AADWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
            List<Medication> medications = new List<Medication>();

            _databaseService.ConnectToMssqlServer(DatabaseService.AvailableDatabases.program_data);

            //GET medications TABLE
            var result = _databaseService.RetrieveTable("medications", "id", id);

            while (result.Read())
            {
                medications.Add(new Medication
                {
                    id = (Int16)result.GetValue(0),
                    medication = (string)result.GetValue(1),
                    blood_work_restriction_level = Convert.IsDBNull(result.GetValue(2)) ? (short?)null : (Int16)result.GetValue(2)
                });
            }

            return medications.AsEnumerable();
        }
    }
}
