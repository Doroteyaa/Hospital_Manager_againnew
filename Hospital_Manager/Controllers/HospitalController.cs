using Data_Hospital_Manager;
using Data_Hospital_Manager.Entities;
using Hospital_Manager.Core.Interfaces;
using Hospital_Manager.Core.Services;
using Hospital_Manager.ViewModels.Hospital;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Manager.Controllers
{

    public class HospitalController : Controller
    {

        private readonly IHospitalService hs;

        public HospitalController(IHospitalService hospitalService)
        {
            hs = hospitalService;
        }
      
        public async Task<IActionResult> Index()
        {
            var hospitals = await hs.GetAllHospitalsAsync();

            var model = new List<HospitalIndexViewModel>();//празен списък за ViewModel-ите

            foreach (var hospital in hospitals)
            {
                model.Add(new HospitalIndexViewModel
                {
                    Id = hospital.Id,
                    Name = hospital.Name,
                    Address = hospital.Address,
                    Email = hospital.Email,
                    PhoneNumber = hospital.PhoneNumber,
                    BedsCount = hospital.BedsCount
                });
            }

            return View(model);//Изпращаме списъка към View
        }
        

        public async Task<IActionResult> Details(int id)
        {
            var hospital = await hs.GetHospitalByIdAsync(id);

            if (hospital == null)
            {
                return NotFound();
            }

            var model = new HospitalDetailsViewModel
            {
                Id = hospital.Id,
                Name = hospital.Name,
                Address = hospital.Address,
                Email = hospital.Email,
                PhoneNumber = hospital.PhoneNumber,
                BedsCount = hospital.BedsCount,
                CreatedOn = hospital.CreatedOn
            };

            return View(model);
        }

        public IActionResult Create()//метод  който показва празната форма.
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(HospitalCreateViewModel model)//обработва POST заявка
        {
            if (ModelState.IsValid)//Проверява валидацията
            {
                var hospital = new Hospital
                {
                    Name = model.Name,
                    Address = model.Address,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    BedsCount = model.BedsCount,
                    CreatedOn = DateTime.Now,
                    IsDeleted = false
                };

                hs.AddHospitalAsync(hospital);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var hospital = await hs.GetHospitalByIdAsync(id);

            if (hospital == null)
            {
                return NotFound();
            }

            var model = new HospitalEditViewModel
            {
                Id = hospital.Id,
                Name = hospital.Name,
                Address = hospital.Address,
                Email = hospital.Email,
                PhoneNumber = hospital.PhoneNumber,
                BedsCount = hospital.BedsCount
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, HospitalEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var hospital = await hs.GetHospitalByIdAsync(id);

                if (hospital == null)
                {
                    return NotFound();
                }

                hospital.Name = model.Name;
                hospital.Address = model.Address;
                hospital.Email = model.Email;
                hospital.PhoneNumber = model.PhoneNumber;
                hospital.BedsCount = model.BedsCount;
                await hs.UpdateHospitalAsync(hospital);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)//Сигурни ли сте, че искате да изтриете тази болница?
        {
            var hospital = await hs.GetHospitalByIdAsync(id);

            if (hospital == null)
            {
                return NotFound();
            }

            var model = new HospitalDeleteViewModel
            {
                Id = hospital.Id,
                Name = hospital.Name
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             await hs.DeleteHospitalAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}


