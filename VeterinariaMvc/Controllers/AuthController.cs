using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VeterinariaMvc.Dtos.Auth;
using VeterinariaMvc.Mappers;
using VeterinariaMvc.Models;
using VeterinariaMvc.Services.Auth;
using VeterinariaMvc.Services.Estado;
using VeterinariaMvc.Services.UsuarioService;

namespace VeterinariaMvc.Controllers
{
    public class AuthController : Controller
    {
        private IAuthService authService;
        private IEstadoUsuarioService estadoUsuarioService;

        public AuthController
            (IAuthService authService, IEstadoUsuarioService estadoUsuarioService)
        {
            this.authService = authService;
            this.estadoUsuarioService = estadoUsuarioService;
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

            if (usuario != null)
            {

                //Guardar usuario IEstadoUsuario
                await this.estadoUsuarioService.GuardarSesionAsync(usuario.ToSessionDto());

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
