using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VeterinariaMvc.Dtos.Auth;
using VeterinariaMvc.Enums;
using VeterinariaMvc.Mappers;
using VeterinariaMvc.Models;
using VeterinariaMvc.Models.Enums;
using VeterinariaMvc.Services.Auth;
using VeterinariaMvc.Services.Estado;
using VeterinariaMvc.Services.Imagenes;
using VeterinariaMvc.Services.UsuarioService;

namespace VeterinariaMvc.Controllers
{
    public class AuthController : Controller
    {
        private IAuthService authService;
        private IUsuarioService usuarioService;
        private IEstadoUsuarioService estadoUsuarioService;

        public AuthController
            (IAuthService authService,
            IEstadoUsuarioService estadoUsuarioService,
            IUsuarioService usuarioService)
        {
            this.authService = authService;
            this.estadoUsuarioService = estadoUsuarioService;
            this.usuarioService = usuarioService;
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
                return RedirectToAction("Index", "Home");   
            }
            else
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
                return View(request);
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return View(registerDto);
            registerDto.Rol = Roles.Usuario;
            try
            {
                Usuario? usuario = await this.authService.RegisterUsuarioAsync(registerDto);

                if (usuario != null)
                {
                    await this.estadoUsuarioService.GuardarSesionAsync(usuario.ToSessionDto());
                    return RedirectToAction("Index", "Home");
                }

                ViewData["ERROR"] = "No se pudo crear la cuenta. Inténtalo de nuevo.";
                return View(registerDto);
            }
            catch (InvalidOperationException ex)
            {
                ViewData["ERROR"] = ex.Message;
                return View(registerDto);
            }
            catch
            {
                ViewData["ERROR"] = "Ha ocurrido un error al registrar el usuario.";
                return View(registerDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.estadoUsuarioService.DestruirSesionAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
