using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VeterinariaMvc.Dtos.Auth;
using VeterinariaMvc.Models;
using VeterinariaMvc.Services.Auth;
using VeterinariaMvc.Services.UsuarioService;

namespace VeterinariaMvc.Controllers
{
    public class AuthController : Controller
    {
        private IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto request)
        {
            Usuario usuario = await this.authService
                .LoginAsync(request.Email, request.Password);

            if(usuario != null)
            {
                //ALMACENAJE EN COOKIES, SESSION ETC

                //REDIRECCION SEGUN EL ROL



                ViewData["MENSAJE"] = "LOGIN CORRECTO";

            }
            else
            {
                ViewData["MENSAJE"] = "ERROR";

            }

            return View();
        }
    }
}
