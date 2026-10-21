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
    public class DoctorService:IDoctorInterface
    {
        private readonly HospitalDbContext context;

        public DoctorService(HospitalDbContext context)
        {
            this.context = context;
        }

        public async Task AddDoctorAsync(Doctor doctor)
        {
          context.Doctors.Add(doctor);
            await context.SaveChangesAsync();
        }

        public async Task DeleteDoctorAsync(int id)
        {
            var doctor = await context.Doctors.Include(x=>x.DPatients)
                .FirstOrDefaultAsync(x=>x.Id==id);
            if (doctor != null)
            {
                context.DoctorPatients.RemoveRange(doctor.DPatients);
                context.Doctors.Remove(doctor);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            return await context.Doctors.Include(x=>x.Hospital).ToListAsync();
        }

        public async Task<Doctor?> GetDoctorByIdAsync(int id)
        {
            return await context.Doctors
                .Include(d => d.DPatients)
                .Include(d => d.Hospital)
                .FirstOrDefaultAsync();
        }

        public async Task<Doctor?> GetDoctorDetailsAsync(int id)
        {
            return await context.Doctors.Include(d=>d.Hospital)
                .Include(x=>x.DPatients).ThenInclude(x=>x.Patient)
                .FirstOrDefaultAsync(d=>d.Id==id);  
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            await context.SaveChangesAsync();
        }
    }
}
