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

        public Task AddHospitalAsync(Hospital hospital)
        {
      
        }

        public async Task DeleteHospitalAsync(int id)
        {
           
        }

        public async Task<Hospital?> GetAllHospitalByIdAsync(int id)
        {
           
        }

        public async Task<List<Hospital>> GetAllHospitalsAsync()
        {
            return await context.Hospitals.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task UpdateHospitalAsync(Hospital hospital)
        {
          
        }
    }
}
