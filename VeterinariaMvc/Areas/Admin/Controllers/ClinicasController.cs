using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeterinariaMvc.Areas.Admin.Models;
using VeterinariaMvc.Models;
using VeterinariaMvc.Services.Clinica;

namespace VeterinariaMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "1")] 
    public class ClinicasController : Controller
    {
        private readonly IClinicaService _clinicaService;

        public ClinicasController(IClinicaService clinicaService)
        {
            _clinicaService = clinicaService;
        }

        public async Task<IActionResult> Index()
        {
            var clinicas = await _clinicaService.GetClinicasAsync();
            return View(clinicas);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new RegistroClinicaViewModel
            {
                HoraApertura = new TimeSpan(9, 0, 0),
                HoraCierre = new TimeSpan(18, 0, 0),
                DuracionCita = 30
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegistroClinicaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                Clinica nuevaClinica = new Clinica
                {
                    Nombre = model.NombreClinica,
                    Direccion = model.Direccion,
                    Telefono = model.Telefono,
                    Email = model.Email,
                    Activo = true,
                    Estado = true
                };

                int id = await _clinicaService.RegistrarNuevaClinicaAsync(
                    nuevaClinica,
                    model.Email,
                    model.Password,
                    model.HoraApertura,
                    model.HoraCierre,
                    model.DuracionCita
                );

                TempData["ToastMessage"] = "Clínica y agenda creadas con éxito.";
                TempData["ToastType"] = "success";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["ERROR"] = "No se pudo registrar la clínica: " + ex.Message;
                return View(model);
            }
        }
    }
}