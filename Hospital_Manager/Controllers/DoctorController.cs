using Data_Hospital_Manager;
using Data_Hospital_Manager.Entities;
using Hospital_Manager.Core.Interfaces;
using Hospital_Manager.Core.Services;
using Hospital_Manager.ViewModels.Doctor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Manager.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorInterface ds;
        private readonly IHospitalService hs;

        public DoctorController(IDoctorInterface context, IHospitalService hs)
        {
            this.ds = context;
            this.hs = hs;
        }


        public async Task<IActionResult> Index()
        {
            var doctors = await ds.GetAllDoctorsAsync();

            var model = new List<DoctorIndexViewModel>();

            foreach (var doctor in doctors)
            {
                model.Add(new DoctorIndexViewModel
                {
                    Id = doctor.Id,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    Specialty = doctor.Specialty,
                    HospitalName = doctor.Hospital.Name
                });
            }

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var doctor = await ds.GetDoctorDetailsAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            var model = new DoctorDetailsViewModel
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Specialty = doctor.Specialty,
                Email = doctor.Email,
                PhoneNumber = doctor.PhoneNumber,
                HospitalName = doctor.Hospital.Name,
                Patients = doctor.DPatients
                .Select(x => $"{x.Patient.FirstName} {x.Patient.LastName}").ToList()
            };

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Hospitals = new SelectList(
            await hs.GetAllHospitalsAsync(),
             "Id",
            "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DoctorCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doctor = new Doctor
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Specialty = model.Specialty,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    HospitalId = model.HospitalId
                };

                ds.AddDoctorAsync(doctor);
                return RedirectToAction(nameof(Index));
            }
            //Вземи болниците от БД и ги сложи във ViewBag.Hospitals.
                        ViewBag.Hospitals = new SelectList(
                await hs.GetAllHospitalsAsync(),
                "Id",
                "Name");

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await ds.GetDoctorByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            var model = new DoctorEditViewModel
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Specialty = doctor.Specialty,
                Email = doctor.Email,
                PhoneNumber = doctor.PhoneNumber,
                HospitalId = doctor.HospitalId
            };

            ViewBag.Hospitals = new SelectList(
                    await hs.GetAllHospitalsAsync(),
                    "Id",
                    "Name");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, DoctorEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var doctor = await ds.GetDoctorByIdAsync(id);

                if (doctor == null)
                {
                    return NotFound();
                }

                doctor.FirstName = model.FirstName;
                doctor.LastName = model.LastName;
                doctor.Specialty = model.Specialty;
                doctor.Email = model.Email;
                doctor.PhoneNumber = model.PhoneNumber;
                doctor.HospitalId = model.HospitalId;

                await ds.UpdateDoctorAsync(doctor);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Hospitals = new SelectList(
                  await hs.GetAllHospitalsAsync(),
                  "Id",
                  "Name");

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await ds.GetDoctorByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            var model = new DoctorDeleteViewModel
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await ds.DeleteDoctorAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

