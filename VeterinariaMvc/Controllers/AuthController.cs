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

                ViewData["MENSAJE"] = "LOGIN CORRECTO";

            }
            else
            {
                ViewData["MENSAJE"] = "ERROR";
            }

            return View();
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

            //Comprobar si existe imagen -> existe guardar y asignar ruta de la imagen
            string imagen;

            if(registerDto.Imagen != null)
            {
                imagen = await this.imagenService.SubirImagenAsync(registerDto.Imagen, CarpetaDestino.Usuarios);
                ViewData["MENSAJE"] = "SE SUBIO LA FOTO" + imagen;
            }

            //Encriptacion Contraseña

            //Crear usuario nuevo

            //Guardar usuario en IEstadoUsuario y loguearle

            return View();
        }
    }
}
