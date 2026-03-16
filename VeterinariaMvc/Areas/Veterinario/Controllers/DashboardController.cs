using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using VeterinariaMvc.Areas.Veterinario.Models;
using VeterinariaMvc.Services.Consulta;
using VeterinariaMvc.Services.Veterinarios;

namespace VeterinariaMvc.Areas.Veterinario.Controllers
{
    [Area("Veterinario")]
    public class DashboardController : Controller
    {
        private readonly IConsultaVeterinarioService _consultaVeterinarioService;
        private readonly IVeterinarioService _veterinarioService;

        public DashboardController(IConsultaVeterinarioService consultaVeterinarioService, IVeterinarioService veterinarioService)
        {
            _consultaVeterinarioService = consultaVeterinarioService;
            _veterinarioService = veterinarioService;
        }

        public async Task<IActionResult> Index()
            {
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            // Convertimos el ID de usuario al ID de la tabla VETERINARIO
            int idClinica = int.Parse(User.FindFirst("IdClinica")?.Value ?? "0");
            int? idVeterinario = await _veterinarioService.ObtenerIdVeterinarioAsync(idUsuario, idClinica);

            if (idVeterinario == null)
            {
                // Si por algún motivo no se encuentra el vínculo, mostramos un dashboard vacío
                return View(new DashboardVeterinarioViewModel());
            }

            var modelo = await _consultaVeterinarioService.GetDashboardVeterinarioAsync(idVeterinario.Value);
            return View(modelo);
        }
    }
}
