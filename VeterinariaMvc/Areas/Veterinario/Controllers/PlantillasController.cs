using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using VeterinariaMvc.Areas.Veterinario.Models;
using VeterinariaMvc.Models.Plantillas;
using VeterinariaMvc.Services.Plantillas;

namespace VeterinariaMvc.Areas.Veterinario.Controllers
{
    [Area("Veterinario")]
    public class PlantillasController : Controller
    {
        private readonly IPlantillaService _plantillaService;

        public PlantillasController(IPlantillaService plantillaService)
        {
            _plantillaService = plantillaService;
        }

        private int ObtenerIdClinica()
        {
            return int.Parse(User.FindFirst("IdClinica")?.Value ?? "0");
        }

        public async Task<IActionResult> Index()
        {
            int idClinica = ObtenerIdClinica();
            
            if(idClinica == 0)
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
            int idClinica = ObtenerIdClinica();
            
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
            int idClinica = ObtenerIdClinica();
            var modelo = await _plantillaService.GetPlantillaDetalleAsync(id, idClinica);
            if (modelo == null)
            {
                return NotFound();
            }

            return View(modelo);
        }

        public async Task<IActionResult> Edit(int id)
        {
            int idClinica = ObtenerIdClinica();
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
            int idClinica = ObtenerIdClinica();

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
            int idClinica = ObtenerIdClinica();
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
            int idClinica = ObtenerIdClinica();
            await _plantillaService.DeletePlantillaAsync(id, idClinica);
            return RedirectToAction("Index");
        }
    }
}
