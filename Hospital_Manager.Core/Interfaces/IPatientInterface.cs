using Data_Hospital_Manager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Manager.Core.Interfaces
{
    public interface IPatientInterface
    {
        Task<List<Patient>> GetAllPatientsAsync();
        Task<Patient?> GetPatientByIdAsync(int id);
        Task<Patient?> GetPatientWithDoctorsAsync(int id);
        Task  AddPatientAsync(Patient p);
        Task  UpdatePatientAsync(Patient p,List<int> doctorIds);
        Task DeletePatientAsync(int id);


    }
}
