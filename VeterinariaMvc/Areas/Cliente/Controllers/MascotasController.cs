using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VeterinariaMvc.Areas.Cliente.Models;
using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Dtos.Session;
using VeterinariaMvc.Services.Estado;
using VeterinariaMvc.Services.Mascotas;

namespace VeterinariaMvc.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class MascotasController : Controller
    {
        private IEstadoUsuarioService _estadoUsuario;
        private IMascotasService _mascotasService;

        public MascotasController(IEstadoUsuarioService estadoUsuario, IMascotasService mascotasService)
        {
            this._estadoUsuario = estadoUsuario;
            this._mascotasService = mascotasService;
        }

        public async Task<IActionResult> Index()
        {
            UsuarioSessionDto usuario = await this._estadoUsuario.ObtenerUsuarioActualAsync();

            DashboardViewModel model = new DashboardViewModel();

            List<MascotaResumenDto> mascotas = await this._mascotasService.GetMascotasByUserAsync(usuario.Id);

            model.Mascotas = mascotas;

            return View(model);
        }
        public async Task<IActionResult> Registrar()
        {
            UsuarioSessionDto usuario = await this._estadoUsuario.ObtenerUsuarioActualAsync();

           

            return View();
        }
    }
}
