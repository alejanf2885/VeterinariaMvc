using Microsoft.AspNetCore.Mvc;
using VeterinariaMvc.Services.Estado;
using VeterinariaMvc.Dtos.Session;
using VeterinariaMvc.Services.Mascotas;
using VeterinariaMvc.Areas.Cliente.Models;
using VeterinariaMvc.Dtos.Mascota;

namespace VeterinariaMvc.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private  IEstadoUsuarioService _estadoUsuario;
        private  IMascotasService _mascotasService;

        public HomeController(IEstadoUsuarioService estadoUsuario, IMascotasService mascotasService)
        {
            this._estadoUsuario = estadoUsuario;
            _mascotasService = mascotasService;
        }

        public async Task<IActionResult> Index()
        {
            UsuarioSessionDto usuario = await _estadoUsuario.ObtenerUsuarioActualAsync();

            if (usuario == null) return RedirectToAction("Login", "Auth", new { area = "" });

            List<MascotaResumenDto> mascotas = await _mascotasService.GetMascotasByUserAsync(usuario.Id);

            DashboardViewModel model = new DashboardViewModel();

            model.usuario = usuario;
            model.Mascotas = mascotas;


            return View(model);
        }
    }
}