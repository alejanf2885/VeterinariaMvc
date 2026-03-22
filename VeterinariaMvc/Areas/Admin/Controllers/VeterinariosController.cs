using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VeterinariaMvc.Dtos.Auth;
using VeterinariaMvc.Dtos.Veterinarios;
using VeterinariaMvc.Models;
using VeterinariaMvc.Services.Auth;
using VeterinariaMvc.Services.Estado;
using VeterinariaMvc.Services.Veterinarios;

namespace VeterinariaMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "1")]
    public class VeterinariosController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IVeterinarioService _veterinarioService;
        private readonly IEstadoUsuarioService _estadoUsuarioService;

        public VeterinariosController(
            IAuthService authService,
            IVeterinarioService veterinarioService,
            IEstadoUsuarioService estadoUsuarioService)
        {
            _authService = authService;
            _veterinarioService = veterinarioService;
            _estadoUsuarioService = estadoUsuarioService;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = await _estadoUsuarioService.ObtenerUsuarioActualAsync();

            if (usuario == null || usuario.IdClinica == null)
                return RedirectToAction("Login", "Auth", new { area = "" });

            int idClinica = usuario.IdClinica.Value;

            var veterinarios = await _veterinarioService
                .ObtenerVeterinariosPorClinicaAsync(idClinica);

            return View(veterinarios);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterVeterinarioDto model)
        {
            ModelState.Remove("IdClinica");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = await _estadoUsuarioService.ObtenerUsuarioActualAsync();

            if (usuario == null || usuario.IdClinica == null)
                return RedirectToAction("Login", "Auth", new { area = "" });

            model.IdClinica = usuario.IdClinica.Value;

            Usuario usuarioCreado = await _authService
                .RegistrarVeterinarioAsync(model);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int idUsuario)
        {
            var usuario = await _estadoUsuarioService.ObtenerUsuarioActualAsync();

            if (usuario == null || usuario.IdClinica == null)
                return RedirectToAction("Login", "Auth", new { area = "" });

            int idClinica = usuario.IdClinica.Value;

            await _veterinarioService
                .EliminarVeterinarioAsync(idUsuario, idClinica);

            return RedirectToAction("Index");
        }
    }
}