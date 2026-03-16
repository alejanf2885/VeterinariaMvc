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
        public async Task<IActionResult> Create(RegistroClinicaViewModel model)
        {
            ViewData["DEBUG"] = ""; // Inicializamos la variable de debug

            

            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
           

            int dueño = int.Parse(claim.Value);
            ViewData["DEBUG"] = $"Id del dueño: {dueño}";

      

         

            var idClinicaExistente = await _clinicaService.ObtenerIdClinicaDeUsuarioAsync(dueño);
            ViewData["DEBUG"] += $" | IdClinicaExistente: {idClinicaExistente}";
          

            string rutaFoto = "/images/usuarios/default-avatar.png";
            if (model.Logo != null)
            {
                rutaFoto = await _imagenService.SubirImagenAsync(model.Logo, CarpetaDestino.Clinica, 500);
                ViewData["DEBUG"] += $" | Logo subido a: {rutaFoto}";
            }
            else
            {
                ViewData["DEBUG"] += " | No se subió logo, se usará default";
            }

            try
            {
                Clinica nuevaClinica = new Clinica
                {
                    Nombre = model.NombreClinica,
                    Direccion = model.Direccion,
                    Telefono = model.Telefono,
                    Email = model.Email,
                    Activo = true,
                    Estado = true,
                    IdUsuario = dueño,
                    Imagen = rutaFoto
                };

                int id = await _clinicaService.RegistrarNuevaClinicaAsync(
                    nuevaClinica,
                    model.Email,
                    model.Password,
                    model.HoraApertura,
                    model.HoraCierre,
                    model.DuracionCita
                );

                ViewData["DEBUG"] += $" | Clínica creada con Id: {id}";
                TempData["ToastMessage"] = "Clínica y agenda creadas con éxito.";
                TempData["ToastType"] = "success";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["ERROR"] = "No se pudo registrar la clínica: " + ex.Message;
                ViewData["DEBUG"] += " | Excepción: " + ex.ToString();
                return View(model);
            }
        }
    }
}