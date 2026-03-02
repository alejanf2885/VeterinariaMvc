using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VeterinariaMvc.Dtos.Auth;
using VeterinariaMvc.Enums;
using VeterinariaMvc.Mappers;
using VeterinariaMvc.Models;
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
        private IImagenService imagenService;

        public AuthController
            (IAuthService authService,
            IEstadoUsuarioService estadoUsuarioService,
            IUsuarioService usuarioService,
            IImagenService imagenService)
        {
            this.authService = authService;
            this.estadoUsuarioService = estadoUsuarioService;
            this.usuarioService = usuarioService;
            this.imagenService = imagenService;
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
            {
                return View(registerDto);
            }

            //Comprobar que no exista email
            if(await this.usuarioService.ExisteEmailAsync(registerDto.Email))
            {
                ViewData["ERROR"] = "El correo electronico ya está en uso";
                return View(registerDto);
            }

         

            //Crear usuario nuevo
            Usuario usuario = await this.authService.RegisterAsync
                 (registerDto.Email, registerDto.Password, registerDto.Nombre, registerDto.Telefono, registerDto.Imagen);


            if (usuario != null)
            {
                await this.estadoUsuarioService.GuardarSesionAsync(usuario.ToSessionDto());
                return RedirectToAction("Index", "Home");
            }

            ViewData["ERROR"] = "No se pudo crear la cuenta. Inténtalo de nuevo.";
            return View(registerDto);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.estadoUsuarioService.DestruirSesionAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
