using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using VeterinariaMvc.Areas.Veterinario.Models;
using VeterinariaMvc.Services.Consulta;
using VeterinariaMvc.Services.Veterinarios;
using VeterinariaMvc.Services.Plantillas;
using VeterinariaMvc.Services.Estado;

namespace VeterinariaMvc.Areas.Veterinario.Controllers
{
    [Area("Veterinario")]
    [Authorize(Roles = "3")]
    public class DashboardController : Controller
    {
        private readonly IConsultaVeterinarioService _consultaVeterinarioService;
        private readonly IVeterinarioService _veterinarioService;
        private readonly IPlantillaService _plantillaService;
        private readonly IEstadoUsuarioService _estadoUsuarioService;

        public DashboardController(
            IConsultaVeterinarioService consultaVeterinarioService,
            IVeterinarioService veterinarioService,
            IPlantillaService plantillaService,
            IEstadoUsuarioService estadoUsuarioService)
        {
            _consultaVeterinarioService = consultaVeterinarioService;
            _veterinarioService = veterinarioService;
            _plantillaService = plantillaService;
            _estadoUsuarioService = estadoUsuarioService;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = await _estadoUsuarioService.ObtenerUsuarioActualAsync();

            if (usuario == null || !usuario.IdClinica.HasValue)
            {
                return View(new DashboardVeterinarioViewModel());
            }

            int idUsuario = usuario.Id;
            int idClinica = usuario.IdClinica.Value;

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