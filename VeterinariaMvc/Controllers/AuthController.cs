using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VeterinariaMvc.Dtos.Auth;
using VeterinariaMvc.Models;
using VeterinariaMvc.Services.UsuarioService;

namespace VeterinariaMvc.Controllers
{
    public class AuthController : Controller
    {
        private IUsuarioService usuarioService;

        public AuthController(IUsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto request)
        {
            Usuario usuario = await this.usuarioService
                .ValidarYObtenerUsuarioAsync(request.Email, request.Password);

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
