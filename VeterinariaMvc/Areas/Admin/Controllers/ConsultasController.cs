using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VeterinariaMvc.Services.Consulta;
using VeterinariaMvc.Services.Veterinarios;

namespace VeterinariaMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ConsultasController : Controller
    {
        private readonly IConsultaService _consultaService;
        private readonly IVeterinarioService _veterinarioService;

        public ConsultasController(IConsultaService consultaService, IVeterinarioService veterinarioService)
        {
            _consultaService = consultaService;
            _veterinarioService = veterinarioService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AsignarVeterinario(int idConsulta, int idUsuarioVeterinario)
        {
            int idClinica = int.Parse(User.FindFirst("IdClinica")?.Value ?? "0");

            int? idVeterinario = await _veterinarioService.ObtenerIdVeterinarioAsync(idUsuarioVeterinario, idClinica);

            if (idVeterinario == null)
            {
                TempData["ERROR"] = "No se encontró el veterinario para esta clínica.";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            bool ok = await _consultaService.AsignarVeterinarioAsync(idConsulta, idVeterinario.Value);

            if (!ok)
            {
                TempData["ERROR"] = "No se pudo asignar el veterinario a la cita.";
            }

            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
    }
}
