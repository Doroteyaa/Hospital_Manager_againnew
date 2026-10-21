using Data_Hospital_Manager;
using Data_Hospital_Manager.Entities;
using Hospital_Manager.Core.Interfaces;
using Hospital_Manager.Core.Services;
using Hospital_Manager.Models;
using Hospital_Manager.ViewModels.Hospital;
using Hospital_Manager.ViewModels.Patient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Hospital_Manager.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientInterface ps;
        private readonly IDoctorInterface ds;

        public PatientController(IPatientInterface ps, IDoctorInterface ds)
        {
            this.ps = ps;
            this.ds = ds;
        }

        public async Task<IActionResult> Index()
        {
            var patients = await ps.GetAllPatientsAsync();
            var model = new List<PatientIndexViewModel>();
            foreach (var patient in patients)
            {
                model.Add(new PatientIndexViewModel
                {
                    Id = patient.Id,
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    Email = patient.Email,
                    PhoneNumber = patient.PhoneNumber,
                    DoctorName = string.Join(" ",
                    patient.DPatients
                    .Select(dp => dp.Doctor.FirstName + " " + dp.Doctor.LastName))
                });
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(PatientCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var patient = new Patient
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };

                foreach (var item in model.DoctorIds)
                {
                    patient.DPatients.Add(new DoctorPatient
                    {
                        DoctorId=item
                    });
                }
                ps.AddPatientAsync(patient);
                return RedirectToAction(nameof(Index));
            }
            await LoadDoctors(model.DoctorIds);
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await ps.GetPatientWithDoctorsAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            var model = new PatientEditViewModel
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                DoctorIds = patient.DPatients.Select(x => x.DoctorId).ToList()
            };
            await LoadDoctors(model.DoctorIds);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, PatientEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var p = await ps.GetPatientWithDoctorsAsync(id);
                if (p == null)
                {
                    return NotFound();
                }
                p.FirstName= model.FirstName;
                p.LastName= model.LastName;
                p.Email= model.Email;   
                p.PhoneNumber= model.PhoneNumber;
                p.DPatients.Clear();
                foreach (var item in model.DoctorIds)
                {
                    p.DPatients.Add(new DoctorPatient
                    {
                        DoctorId=item,
                        PatientId=p.Id
                    });
                }
                await ps.UpdatePatientAsync(p, model.DoctorIds);
                return RedirectToAction(nameof(Index));
            }
            await LoadDoctors();
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            var patient = await ps.GetPatientWithDoctorsAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            var model = new PatientDetailsViewModel
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                DoctorName = string.Join(" ",
                    patient.DPatients
                    .Select(dp => dp.Doctor.FirstName + " " + dp.Doctor.LastName))
            };
            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            await LoadDoctors();
            return View();
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await ps.DeletePatientAsync(id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var p = await ps.GetPatientByIdAsync(id);
            if (p == null)
            {
                return NotFound();
            }

            var model = new PatientDeleteViewModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,

            };
            return View(model);
        }

        private async Task LoadDoctors(List<int> selectedD = null)
        {
            var doctors = await ds.GetAllDoctorsAsync();
           var doctori = doctors
            .Select(d => new
            {
                d.Id,
                FullName = d.FirstName + " " + d.LastName
            }).ToList();
            ViewBag.Docotors = new MultiSelectList(
            doctori,
            "Id",
            "FullName",
            selectedD
            );
        }
    }
}
