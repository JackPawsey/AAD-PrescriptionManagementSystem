using System;
using System.Collections.Generic;
using AADWebApp.Models;
using static AADWebApp.Services.PrescriptionService;

namespace AADWebApp.Interfaces
{
    public interface IPrescriptionService
    {
        public IEnumerable<Prescription> GetPrescriptions(short? id = null);
        public int CreatePrescription(int MedicationId, string PatientId, int Dosage, DateTime DateStart, DateTime DateEnd, PrescriptionStatus PrescriptionStatus, string IssueFrequency);
        public int CancelPrescription(int Id);
        public int SetPrescriptionStatus(int Id, PrescriptionStatus PrescriptionStatus);
    }
}