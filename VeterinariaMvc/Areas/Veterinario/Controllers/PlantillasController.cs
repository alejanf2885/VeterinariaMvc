using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VeterinariaMvc.Areas.Veterinario.Models;
using VeterinariaMvc.Models.Plantillas;
using VeterinariaMvc.Services.Plantillas;
using VeterinariaMvc.Services.Estado;

namespace VeterinariaMvc.Areas.Veterinario.Controllers
{
    [Area("Veterinario")]
    [Authorize(Roles = "3")]
    public class PlantillasController : Controller
    {
        private readonly IPlantillaService _plantillaService;
        private readonly IEstadoUsuarioService _estadoUsuarioService;

        public PlantillasController(
            IPlantillaService plantillaService,
            IEstadoUsuarioService estadoUsuarioService)
        {
            _plantillaService = plantillaService;
            _estadoUsuarioService = estadoUsuarioService;
        }

        private async Task<int> ObtenerIdClinicaAsync()
        {
            var usuario = await _estadoUsuarioService.ObtenerUsuarioActualAsync();
            return usuario?.IdClinica ?? 0;
        }

        public async Task<IActionResult> Index()
        {
            int idClinica = await ObtenerIdClinicaAsync();

            if (idClinica == 0)
            {
                return View(new List<Plantilla>());
            }

            var plantillas = await _plantillaService.GetPlantillasPorClinicaAsync(idClinica);
            return View(plantillas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearPlantillaViewModel model)
        {
            int idClinica = await ObtenerIdClinicaAsync();

            if (idClinica == 0)
            {
                ModelState.AddModelError("", "No se pudo identificar la clínica a la que perteneces.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _plantillaService.CrearPlantillaCompletaAsync(model, idClinica);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            int idClinica = await ObtenerIdClinicaAsync();
            var modelo = await _plantillaService.GetPlantillaDetalleAsync(id, idClinica);
            if (modelo == null)
            {
                return NotFound();
            }

            return View(modelo);
        }

        public async Task<IActionResult> Edit(int id)
        {
            int idClinica = await ObtenerIdClinicaAsync();
            var detalle = await _plantillaService.GetPlantillaDetalleAsync(id, idClinica);
            if (detalle == null)
            {
                return NotFound();
            }

            var vm = new CrearPlantillaViewModel
            {
                Nombre = detalle.Nombre,
                Activa = detalle.Activa
            };

            ViewBag.IdPlantilla = id;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CrearPlantillaViewModel model)
        {
            int idClinica = await ObtenerIdClinicaAsync();

            if (!ModelState.IsValid)
            {
                ViewBag.IdPlantilla = id;
                return View(model);
            }

            bool ok = await _plantillaService.UpdatePlantillaBasicaAsync(id, idClinica, model.Nombre, model.Activa);
            if (!ok)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            int idClinica = await ObtenerIdClinicaAsync();
            var detalle = await _plantillaService.GetPlantillaDetalleAsync(id, idClinica);
            if (detalle == null)
            {
                return NotFound();
            }

            return View(detalle);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int idClinica = await ObtenerIdClinicaAsync();
            await _plantillaService.DeletePlantillaAsync(id, idClinica);
            return RedirectToAction("Index");
        }
    }
}