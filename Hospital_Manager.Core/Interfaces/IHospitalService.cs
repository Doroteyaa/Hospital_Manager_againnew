using Data_Hospital_Manager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Manager.Core.Interfaces
{
    public interface IHospitalService
    {
        Task<List<Hospital>> GetAllHospitalsAsync();
        Task<Hospital?> GetAllHospitalByIdAsync(int id);
        Task  AddHospitalAsync(Hospital hospital);
        Task  DeleteHospitalAsync(int id);
        Task  UpdateHospitalAsync(Hospital hospital);
     

    }
}
