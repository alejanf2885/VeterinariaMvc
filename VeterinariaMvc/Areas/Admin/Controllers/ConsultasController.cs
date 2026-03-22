using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VeterinariaMvc.Services.Consulta;
using VeterinariaMvc.Services.Veterinarios;
using VeterinariaMvc.Services.Estado;

namespace VeterinariaMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "1")]
    public class ConsultasController : Controller
    {
        private readonly IConsultaService _consultaService;
        private readonly IVeterinarioService _veterinarioService;
        private readonly IEstadoUsuarioService _estadoUsuarioService;

        public ConsultasController(
            IConsultaService consultaService,
            IVeterinarioService veterinarioService,
            IEstadoUsuarioService estadoUsuarioService)
        {
            _consultaService = consultaService;
            _veterinarioService = veterinarioService;
            _estadoUsuarioService = estadoUsuarioService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AsignarVeterinario(int idConsulta, int idUsuarioVeterinario)
        {
            var usuario = await _estadoUsuarioService.ObtenerUsuarioActualAsync();

            if (usuario == null || usuario.IdClinica == null)
            {
                TempData["ERROR"] = "No se pudo obtener la clínica del usuario.";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            int idClinica = usuario.IdClinica.Value;

            int? idVeterinario = await _veterinarioService
                .ObtenerIdVeterinarioAsync(idUsuarioVeterinario, idClinica);

            if (idVeterinario == null)
            {
                TempData["ERROR"] = "No se encontró el veterinario para esta clínica.";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            bool ok = await _consultaService
                .AsignarVeterinarioAsync(idConsulta, idVeterinario.Value);

            if (!ok)
            {
                TempData["ERROR"] = "No se pudo asignar el veterinario a la cita.";
            }

            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
    }
}