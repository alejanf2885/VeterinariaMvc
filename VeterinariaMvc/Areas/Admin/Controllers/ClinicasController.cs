using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeterinariaMvc.Areas.Admin.Models;
using VeterinariaMvc.Enums;
using VeterinariaMvc.Models;
using VeterinariaMvc.Services.Clinica;
using VeterinariaMvc.Services.Imagenes;
using VeterinariaMvc.Services.Estado;

namespace VeterinariaMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClinicasController : Controller
    {
        private readonly IClinicaService _clinicaService;
        private readonly IImagenService _imagenService;
        private readonly IEstadoUsuarioService _estadoUsuarioService;

        public ClinicasController(
            IClinicaService clinicaService,
            IImagenService imagenService,
            IEstadoUsuarioService estadoUsuarioService)
        {
            _clinicaService = clinicaService;
            _imagenService = imagenService;
            _estadoUsuarioService = estadoUsuarioService;
        }
        [Authorize(Roles = "AdminClinica")]

        public async Task<IActionResult> Index()
        {
            var clinicas = await _clinicaService.GetClinicasAsync();
            return View(clinicas);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new RegistroClinicaViewModel
            {
                HoraAperturaStr = "09:00",
                HoraCierreStr = "18:00",
                DuracionCita = 30
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegistroClinicaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var usuario = await _estadoUsuarioService.ObtenerUsuarioActualAsync();

                if (usuario == null)
                    return RedirectToAction("Login", "Auth");

                int dueñoId = usuario.Id;

                string rutaFoto = "/images/usuarios/default-avatar.png";
                if (model.Logo != null)
                {
                    rutaFoto = await _imagenService.SubirImagenAsync(
                        model.Logo,
                        CarpetaDestino.Clinica,
                        500
                    );
                }

                Clinica nuevaClinica = new Clinica
                {
                    Nombre = model.NombreClinica,
                    Direccion = model.Direccion,
                    Telefono = model.Telefono ?? "",
                    Email = model.Email,
                    Activo = true,
                    Estado = true,
                    IdUsuario = dueñoId,
                    Imagen = rutaFoto
                };

                await _clinicaService.RegistrarNuevaClinicaAsync(
                    nuevaClinica,
                    model.Email,
                    model.HoraApertura,
                    model.HoraCierre,
                    model.DuracionCita
                );

                return RedirectToAction("Login", "Auth", new { area = "" });
            }
            catch (Exception ex)
            {
                ViewData["ERROR"] = ex.Message;
                return View(model);
            }
        }
    }
}