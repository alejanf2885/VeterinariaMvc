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

        public VeterinariosController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Index()
        {
            return View();
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
    }
}
