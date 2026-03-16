using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeterinariaMvc.Areas.Admin.Models;
using VeterinariaMvc.Enums;
using VeterinariaMvc.Models;
using VeterinariaMvc.Services.Clinica;
using VeterinariaMvc.Services.Imagenes;

namespace VeterinariaMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "1")]
    public class ClinicasController : Controller
    {
        private readonly IClinicaService _clinicaService;
        private readonly IImagenService _imagenService;

        public ClinicasController(IClinicaService clinicaService, IImagenService imagenService)
        {
            _clinicaService = clinicaService;
            _imagenService = imagenService;
        }

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
            // Si el modelo falla, recuperamos los errores para verlos en pantalla
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (claim == null) return RedirectToAction("Login", "Auth");
                int dueñoId = int.Parse(claim.Value);

                string rutaFoto = "/images/usuarios/default-avatar.png";
                if (model.Logo != null)
                {
                    rutaFoto = await _imagenService.SubirImagenAsync(model.Logo, CarpetaDestino.Clinica, 500);
                }

                Clinica nuevaClinica = new Clinica
                {
                    Nombre = model.NombreClinica,
                    Direccion = model.Direccion,
                    Telefono = model.Telefono ?? "",
                    Email = model.Email,
                    Activo = true,
                    Estado = true,
                    IdUsuario = dueñoId, // Lo vinculamos al usuario actual
                    Imagen = rutaFoto
                };

                // LLAMADA AL SERVICIO: Aquí quité el parámetro Password si ya no lo usas
                await _clinicaService.RegistrarNuevaClinicaAsync(
                    nuevaClinica,
                    model.Email,
                    model.HoraApertura,
                    model.HoraCierre,
                    model.DuracionCita
                );

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["ERROR"] = ex.Message;
                return View(model);
            }
        }
    }
}