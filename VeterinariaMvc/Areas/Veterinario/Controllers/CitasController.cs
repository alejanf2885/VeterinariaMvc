using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using VeterinariaMvc.Services.Consulta;

namespace VeterinariaMvc.Areas.Veterinario.Controllers
{
    [Area("Veterinario")]
    public class CitasController : Controller
    {
        private readonly IConsultaVeterinarioService _consultaVeterinarioService;

        public CitasController(IConsultaVeterinarioService consultaVeterinarioService)
        {
            _consultaVeterinarioService = consultaVeterinarioService;
        }

        public async Task<IActionResult> Index()
        {
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var modelo = await _consultaVeterinarioService.GetDashboardVeterinarioAsync(idUsuario);
            return View(modelo);
        }
    }

}