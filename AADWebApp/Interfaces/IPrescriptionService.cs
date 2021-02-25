using System;
using System.Collections.Generic;
using AADWebApp.Models;
using static AADWebApp.Services.PrescriptionService;

namespace AADWebApp.Interfaces
{
    public interface IPrescriptionService
    {
        public IEnumerable<Prescription> GetPrescriptions(short? id = null);
        public IEnumerable<Prescription> GetPrescriptionsByPatientId(string id = null);
        public int CreatePrescription(int medicationId, string patientId, int dosage, DateTime dateStart, DateTime dateEnd, PrescriptionStatus prescriptionStatus, IssueFrequency issueFrequency);
        public int CancelPrescription(int id);
        public int SetPrescriptionStatus(int id, PrescriptionStatus prescriptionStatus);
    }
}