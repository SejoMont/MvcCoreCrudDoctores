using Microsoft.AspNetCore.Mvc;
using MvcCoreCrudDoctores.Models;
using MvcCoreCrudDoctores.Repositories;

namespace MvcCoreCrudDoctores.Controllers
{
    public class DoctoresController : Controller
    {
        RepositoryDoctores repo;
        public DoctoresController()
        {
            this.repo = new RepositoryDoctores();
        }
        public async Task<IActionResult> Index()
        {
            List<Doctor> doctores = await this.repo.GetDoctoresAsync();
            return View(doctores);
        }

        public async Task<IActionResult> Details(int id)
        {
            Doctor doctor = await this.repo.FindDoctorAsync(id);

            return View(doctor);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int hospitalCod, string apellido, string especialidad, int salario)
        {
            await this.repo.InsertDoctorAsync(hospitalCod, apellido, especialidad, salario);
            return RedirectToAction("Index");
        }
        //al mostrar el formulario de update, deberiamos dibujar los datos del departamento que estamos modificando
        public async Task<IActionResult> Edit(int id)
        {
            Doctor doctor = await this.repo.FindDoctorAsync(id);
            return View(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Doctor doctor)
        {
            await this.repo.UpdateDoctorAsync(doctor.DoctorNo, doctor.HospitalCod, doctor.Apellido, doctor.Especialidad, doctor.Salario);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.repo.DeleteDoctorAsync(id);
            return RedirectToAction("Index");
        }
    }
}
