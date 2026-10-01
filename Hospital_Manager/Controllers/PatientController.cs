using Data_Hospital_Manager;
using Microsoft.AspNetCore.Mvc;
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
            var patients=await context.Patients.ToListAsync();
            return View();
        }
    }
}
