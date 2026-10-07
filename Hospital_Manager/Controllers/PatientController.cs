using Data_Hospital_Manager;
using Data_Hospital_Manager.Entities;
using Hospital_Manager.ViewModels.Patient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Hospital_Manager.Controllers
{
    public class PatientController : Controller
    {
        private readonly HospitalDbContext context;

        public PatientController(HospitalDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var patients = await context.Patients
                .Include(x => x.DPatients)
                .ThenInclude(y => y.Doctor)
                .ToListAsync();
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
                context.Patients.Add(patient);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await LoadDoctors(model.DoctorIds);
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await context.Patients
             .Include(x => x.DPatients)
             .ThenInclude(y => y.Doctor).FirstOrDefaultAsync(x => x.Id == id);
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
                var p = await context.Patients.Include(x => x.DPatients).FirstOrDefaultAsync(y => y.Id == id);
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
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await LoadDoctors();
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            var patient = await context.Patients
             .Include(x => x.DPatients)
             .ThenInclude(y => y.Doctor).FirstOrDefaultAsync(x => x.Id == id);
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
            var p=await context.Patients.FindAsync(id);
            if(p != null)
            {
               context.Patients.Remove(p);
                await context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Delete(int id)
        {
            var p = await context.Patients.FindAsync(id);
            if (p == null)
            {
                return NotFound();

            }
            context.Patients.Remove(p);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDoctors(List<int> selectedD = null)
        {
            var doctors = await context.Doctors
            .Select(d => new
            {
                d.Id,
                FullName = d.FirstName + " " + d.LastName
            }).ToListAsync();
            ViewBag.Docotors = new MultiSelectList(
            doctors,
            "Id",
            "FullName",
            selectedD
            );
        }
    }
}
