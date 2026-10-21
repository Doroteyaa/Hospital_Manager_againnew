using Data_Hospital_Manager;
using Data_Hospital_Manager.Entities;
using Hospital_Manager.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Manager.Core.Services
{
    public class PatientService : IPatientInterface
    {
        private readonly HospitalDbContext context;

        public PatientService(HospitalDbContext context)
        {
            this.context = context;
        }

        public async Task AddPatientAsync(Patient p)
        {
           context.Patients.Add(p);
            await context.SaveChangesAsync();
        }

        public async Task DeletePatientAsync(int id)
        {
            var patient = context.Patients.Include(x=>x.DPatients).ThenInclude(x=>x.Doctor).FirstOrDefault(x=>x.Id==id);
            if (patient != null)
            {
                context.DoctorPatients.RemoveRange(patient.DPatients);
                context.Patients.Remove(patient);
                await context.SaveChangesAsync();
            }
        }
        public async Task<List<Patient>> GetAllPatientsAsync()
        {
            return await context.Patients.Include(x=>x.DPatients).ThenInclude(x=>x.Doctor).ToListAsync();
        }
        public async Task<Patient?> GetPatientByIdAsync(int id)
        {
           return await context.Patients.FindAsync(id);
        }
        public async Task<Patient?> GetPatientWithDoctorsAsync(int id)
        {
           return await context.Patients
                .Include(x => x.DPatients)
                .ThenInclude(x => x.Doctor)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async  Task UpdatePatientAsync(Patient p, List<int> doctorIds)
        {
            p.DPatients.Clear();
            foreach (int doctorId in doctorIds.Distinct())
            {
                p.DPatients.Add(new DoctorPatient
                {
                    DoctorId = doctorId ,
                    PatientId = p.Id
                });
            }
            await context.SaveChangesAsync();
        }
    }
}
