﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AADWebApp.Models;
using static AADWebApp.Services.PrescriptionService;

namespace AADWebApp.Interfaces
{
    public interface IPrescriptionService
    {
        public IEnumerable<Prescription> GetPrescriptions(short? id = null);
        public IEnumerable<Prescription> GetPrescriptionsByPatientId(string id = null);
        public int CreatePrescription(short medicationId, string patientId, int dosage, DateTime dateStart, DateTime dateEnd, IssueFrequency issueFrequency);
        public Task<int> CancelPrescriptionAsync(short id);
        public int SetPrescriptionStatus(short id, PrescriptionStatus prescriptionStatus);
    }
}