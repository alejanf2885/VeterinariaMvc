using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VeterinariaMvc.Dtos.Session;
using VeterinariaMvc.Models;
using VeterinariaMvc.Models.Enums;
using VeterinariaMvc.Services.Estado;

namespace VeterinariaMvc.Controllers
{
    public class HomeController : Controller
    {

        private IEstadoUsuarioService _estadoUsuario;

        public HomeController(IEstadoUsuarioService estadoUsuario)
        {
            this._estadoUsuario = estadoUsuario;
        }
        public async Task<IActionResult> Index()
        {
            UsuarioSessionDto usuario = await _estadoUsuario.ObtenerUsuarioActualAsync();

            if (usuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (usuario.IdRol == (int)Roles.AdminClinica)
            {
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }

            if (usuario.IdRol == (int)Roles.Usuario)
            {
                return RedirectToAction("Index", "Home", new { area = "Cliente" });
            }

            if (usuario.IdRol == (int)Roles.Veterinario)
            {
                return RedirectToAction("Index", "Home", new { area = "Veterinario" });
            }

            return RedirectToAction("Login", "Auth");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
