using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using VeterinariaMvc.Areas.Veterinario.Models;
using VeterinariaMvc.Services.Tratamientos;

namespace VeterinariaMvc.Areas.Veterinario.Controllers
{
    [Area("Veterinario")]
    public class TratamientosController : Controller
    {
        private readonly ITratamientoService _tratamientoService;

        public TratamientosController(ITratamientoService tratamientoService)
        {
            _tratamientoService = tratamientoService;
        }

        private int ObtenerIdUsuarioActual()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }

        [HttpGet]
        public IActionResult Create(int idConsulta, int idMascota, string nombreMascota, int idVeterinario)
        {
            int idUsuario = ObtenerIdUsuarioActual();
            // Aquí podrías validar que idUsuario corresponde al usuario del veterinario idVeterinario si lo necesitas

            var model = new CrearTratamientoViewModel
            {
                IdConsulta = idConsulta,
                IdMascota = idMascota,
                NombreMascota = nombreMascota,
                FechaInicio = DateTime.Today,
                IdVeterinario = idVeterinario
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearTratamientoViewModel model)
        {
            int idUsuario = ObtenerIdUsuarioActual();

            if (!ModelState.IsValid)
                return View(model);

            bool ok = await _tratamientoService.CrearTratamientoAsync(
                model.IdMascota,
                model.IdVeterinario,
                model.IdConsulta,
                model.Nombre,
                model.Descripcion,
                model.FechaInicio,
                model.FechaFin
               );

            if (!ok)
            {
                ModelState.AddModelError(string.Empty, "No se pudo crear el tratamiento.");
                return View(model);
            }

            return RedirectToAction("Index", "Dashboard", new { area = "Veterinario" });
        }
    }
}
