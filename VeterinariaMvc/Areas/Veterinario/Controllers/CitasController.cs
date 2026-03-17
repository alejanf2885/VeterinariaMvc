using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VeterinariaMvc.Areas.Veterinario.Models;
using VeterinariaMvc.Services.Consulta;
using VeterinariaMvc.Services.Veterinarios;
using VeterinariaMvc.Services.Plantillas;

namespace VeterinariaMvc.Areas.Veterinario.Controllers
{
    [Area("Veterinario")]
    [Authorize(Roles = "3")]
    public class CitasController : Controller
    {
        private readonly IConsultaVeterinarioService _consultaVeterinarioService;
        private readonly IVeterinarioService _veterinarioService;
        private readonly IPlantillaService _plantillaService;

        public CitasController(IConsultaVeterinarioService consultaVeterinarioService, IVeterinarioService veterinarioService, IPlantillaService plantillaService)
        {
            _consultaVeterinarioService = consultaVeterinarioService;
            _veterinarioService = veterinarioService;
            _plantillaService = plantillaService;
        }

        public async Task<IActionResult> Index()
        {
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            int idClinica = int.Parse(User.FindFirst("IdClinica")?.Value ?? "0");
            int? idVeterinario = await _veterinarioService.ObtenerIdVeterinarioAsync(idUsuario, idClinica);

            if (idVeterinario == null)
            {
                return View(new DashboardVeterinarioViewModel());
            }

            var modelo = await _consultaVeterinarioService.GetDashboardVeterinarioAsync(idVeterinario.Value);
            var plantillas = await _plantillaService.GetPlantillasPorClinicaAsync(idClinica);
            modelo.PlantillasDisponibles = plantillas.Where(p => p.Activa == true).ToList();
            
            return View(modelo);
        }
    }
}