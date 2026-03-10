using Microsoft.AspNetCore.Mvc;
using VeterinariaMvc.Services.Estado;
using VeterinariaMvc.Dtos.Session;
using VeterinariaMvc.Services.Mascotas;
using VeterinariaMvc.Areas.Cliente.Models;
using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Dtos.Consultas.VeterinariaMvc.Dtos.Consulta;
using VeterinariaMvc.Services.Consulta;

namespace VeterinariaMvc.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private  IEstadoUsuarioService _estadoUsuario;
        private  IMascotasService _mascotasService;
        private  IConsultaService _consultaService;

        public HomeController(IEstadoUsuarioService estadoUsuario, IMascotasService mascotasService, IConsultaService consultaService)
        {
            this._estadoUsuario = estadoUsuario;
            _mascotasService = mascotasService;
            _consultaService = consultaService;
        }

        public async Task<IActionResult> Index()
        {
            UsuarioSessionDto usuario = await _estadoUsuario.ObtenerUsuarioActualAsync();

            if (usuario == null) return RedirectToAction("Login", "Auth", new { area = "" });

            List<MascotaResumenDto> mascotas = await _mascotasService.GetMascotasByUserAsync(usuario.Id);
            List<ConsultaResumen> consultas = await _consultaService.GetConsultasDashboardAsync(usuario.Id);
            DashboardViewModel model = new DashboardViewModel();

            model.usuario = usuario;
            model.Mascotas = mascotas;
            model.Consultas = consultas;

            return View(model);
        }
    }
}