using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VeterinariaMvc.Dtos.Auth;
using VeterinariaMvc.Dtos.Session;
using VeterinariaMvc.Enums;
using VeterinariaMvc.Mappers;
using VeterinariaMvc.Models;
using VeterinariaMvc.Models.Enums;
using VeterinariaMvc.Services.Auth;
using VeterinariaMvc.Services.Clinica;
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
        private IClinicaService _clinicaService;

        public AuthController
            (IAuthService authService,
            IEstadoUsuarioService estadoUsuarioService,
            IUsuarioService usuarioService,
            IClinicaService clinicaService
            )
        {
            this.authService = authService;
            this.estadoUsuarioService = estadoUsuarioService;
            this.usuarioService = usuarioService;
            this._clinicaService = clinicaService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto request)
        {
            Usuario usuario = await this.authService.LoginAsync(request.Email, request.Password);

            if (usuario != null)
            {
                UsuarioSessionDto sessionDto = usuario.ToSessionDto();

                sessionDto.IdClinica = await this._clinicaService.ObtenerIdClinicaDeUsuarioAsync(usuario.Id, usuario.IdRol);

                await this.estadoUsuarioService.GuardarSesionAsync(sessionDto);

                if (usuario.IdRol == (int)Roles.AdminClinica)
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                else if (usuario.IdRol == (int)Roles.Veterinario)
                {
                    return RedirectToAction("Index", "Home", new { area = "Veterinario" });
                }

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

            try
            {
                Usuario? usuario = await this.authService.RegisterUsuarioAsync(registerDto);

                if (usuario != null)
                {
                    await this.estadoUsuarioService.GuardarSesionAsync(usuario.ToSessionDto());

                    if (usuario.IdRol == (int)Roles.AdminClinica)
                    {
                        bool clinicaExiste = await this.estadoUsuarioService.EsClinicaConfiguradaAsync(usuario.Id);

                        if (!clinicaExiste)
                        {
                            return RedirectToAction("Create", "Clinicas", new { area = "Admin" });
                        }
                    }

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
