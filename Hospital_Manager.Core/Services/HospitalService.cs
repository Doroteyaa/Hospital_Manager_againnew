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
    public class HospitalService : IHospitalService
    {
        private readonly HospitalDbContext context;

        public HospitalService(HospitalDbContext context)
        {
            this.context = context;
        }

        public async Task AddHospitalAsync(Hospital hospital)
        {
            context.Hospitals.Add(hospital);     
            await context.SaveChangesAsync();
        }

        public async Task DeleteHospitalAsync(int id)
        {
            var hospital = await context.Hospitals.FindAsync(id);
            if (hospital != null)
            {
                hospital.IsDeleted = true;
                await context.SaveChangesAsync();
            }
           
        }

        public async Task<List<Hospital>> GetAllHospitalsAsync()
        {
            return await context.Hospitals.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<Hospital?> GetHospitalByIdAsync(int id)
        {
            return await context.Hospitals.FindAsync(id);
        }

        public async Task UpdateHospitalAsync(Hospital hospital)
        {
            await context.SaveChangesAsync();
        }
    }
}
