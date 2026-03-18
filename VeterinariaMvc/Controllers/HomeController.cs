using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using VeterinariaMvc.Dtos.Session;
using VeterinariaMvc.Models;
using VeterinariaMvc.Models.Enums;
using VeterinariaMvc.Services.Estado;

namespace VeterinariaMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEstadoUsuarioService _estadoUsuario;

        public HomeController(IEstadoUsuarioService estadoUsuario)
        {
            _estadoUsuario = estadoUsuario;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = await _estadoUsuario.ObtenerUsuarioActualAsync();

            if (usuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var rol = (Roles)usuario.IdRol;

            switch (rol)
            {
                case Roles.AdminClinica:
                    bool configurado = await _estadoUsuario.EsClinicaConfiguradaAsync(usuario.Id);
                    if (!configurado)
                    {
                        return RedirectToAction("Create", "Clinicas", new { area = "Admin" });
                    }
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

                case Roles.Usuario:
                    return RedirectToAction("Index", "Home", new { area = "Cliente" });

                case Roles.Veterinario:
                    return RedirectToAction("Index", "Dashboard", new { area = "Veterinario" });

                default:
                    return RedirectToAction("Login", "Auth");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}