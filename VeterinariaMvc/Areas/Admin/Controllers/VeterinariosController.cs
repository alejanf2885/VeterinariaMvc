using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using VeterinariaMvc.Dtos.Auth;
using VeterinariaMvc.Dtos.Veterinarios;
using VeterinariaMvc.Models;
using VeterinariaMvc.Services.Auth;

namespace VeterinariaMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VeterinariosController : Controller
    {
        private readonly IAuthService _authService;
        private readonly VeterinariaMvc.Services.Veterinarios.IVeterinarioService _veterinarioService;

        public VeterinariosController(IAuthService authService, VeterinariaMvc.Services.Veterinarios.IVeterinarioService veterinarioService)
        {
            _authService = authService;
            _veterinarioService = veterinarioService;
        }

        public async Task<IActionResult> Index()
        {
            int idClinica = int.Parse(User.FindFirst("IdClinica")?.Value ?? "0");
            var veterinarios = await _veterinarioService.ObtenerVeterinariosPorClinicaAsync(idClinica);
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

            int idClinica = int.Parse(User.FindFirst("IdClinica")?.Value ?? "0");
            model.IdClinica = idClinica;

            Usuario usuario = await this._authService.RegistrarVeterinarioAsync(model);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int idUsuario)
        {
            int idClinica = int.Parse(User.FindFirst("IdClinica")?.Value ?? "0");
            await _veterinarioService.EliminarVeterinarioAsync(idUsuario, idClinica);
            return RedirectToAction("Index");
        }
    }
}
