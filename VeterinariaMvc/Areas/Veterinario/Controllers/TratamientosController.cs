using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using VeterinariaMvc.Areas.Veterinario.Models;
using VeterinariaMvc.Services.Tratamientos;
using VeterinariaMvc.Services.Plantillas;

namespace VeterinariaMvc.Areas.Veterinario.Controllers
{
    [Area("Veterinario")]
    [Authorize(Roles = "3")]
    public class TratamientosController : Controller
    {
        private readonly ITratamientoService _tratamientoService;
        private readonly IPlantillaService _plantillaService;

        public TratamientosController(ITratamientoService tratamientoService, IPlantillaService plantillaService)
        {
            _tratamientoService = tratamientoService;
            _plantillaService = plantillaService;
        }

        private int ObtenerIdUsuarioActual()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }

        private int ObtenerIdClinicaActual()
        {
            return int.Parse(User.FindFirst("IdClinica")?.Value ?? "0");
        }

        [HttpGet]
        public IActionResult Create(int idConsulta, int idMascota, string nombreMascota, int idVeterinario)
        {
            int idUsuario = ObtenerIdUsuarioActual();

            CrearTratamientoViewModel model = new CrearTratamientoViewModel
            {
                IdConsulta = idConsulta,
                IdMascota = idMascota,
                NombreMascota = nombreMascota,
                FechaInicio = DateTime.Today,
                IdVeterinario = idVeterinario
            };

            return View(model);
        }

        public async Task<IActionResult> Ficha(int idConsulta, int idPlantilla)
        {
            int idClinica = ObtenerIdClinicaActual();
            var detallePlantilla = await _plantillaService.GetPlantillaDetalleAsync(idPlantilla, idClinica);
            if (detallePlantilla == null)
            {
                return NotFound();
            }

            var valoresExistentes = await _plantillaService.GetValoresFichaPorConsultaAsync(idConsulta);

            var vm = new FichaConsultaViewModel
            {
                IdConsulta = idConsulta,
                IdPlantilla = idPlantilla,
                NombrePlantilla = detallePlantilla.Nombre
            };

            foreach (var sec in detallePlantilla.Secciones)
            {
                var secVm = new FichaConsultaSeccionInput
                {
                    NombreSeccion = sec.Nombre
                };

                foreach (var campo in sec.Campos)
                {
                    var valor = valoresExistentes.FirstOrDefault(v => v.IdCampo == campo.Id);

                    var campoVm = new FichaConsultaCampoInput
                    {
                        IdCampo = campo.Id,
                        NombreCampo = campo.Nombre,
                        TipoDato = campo.TipoDato,
                        Obligatorio = campo.Obligatorio ?? false,
                        Opciones = campo.Opciones ?? new System.Collections.Generic.List<string>()
                    };

                    if (valor != null)
                    {
                        campoVm.ValorTexto = valor.ValorTexto;
                        campoVm.ValorNumero = valor.ValorNumero;
                        campoVm.ValorFecha = valor.ValorFecha;
                        campoVm.ValorBooleano = valor.ValorBooleano;
                    }

                    secVm.Campos.Add(campoVm);
                }

                vm.Secciones.Add(secVm);
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ficha(FichaConsultaViewModel model)
        {
            var valoresTexto = new System.Collections.Generic.Dictionary<int, string?>();
            var valoresNumero = new System.Collections.Generic.Dictionary<int, decimal?>();
            var valoresFecha = new System.Collections.Generic.Dictionary<int, System.DateTime?>();
            var valoresBooleano = new System.Collections.Generic.Dictionary<int, bool?>();

            foreach (var sec in model.Secciones)
            {
                foreach (var campo in sec.Campos)
                {
                    switch (campo.TipoDato)
                    {
                        case "Texto":
                        case "Parrafo":
                        case "Seleccion":
                            valoresTexto[campo.IdCampo] = campo.ValorTexto;
                            break;
                        case "Numero":
                            valoresNumero[campo.IdCampo] = campo.ValorNumero;
                            break;
                        case "Fecha":
                            valoresFecha[campo.IdCampo] = campo.ValorFecha;
                            break;
                        case "Booleano":
                            valoresBooleano[campo.IdCampo] = campo.ValorBooleano;
                            break;
                    }
                }
            }

            await _plantillaService.CrearOActualizarFichaConsultaDesdePlantillaAsync(
                model.IdConsulta,
                model.IdPlantilla,
                valoresTexto,
                valoresNumero,
                valoresFecha,
                valoresBooleano);

            return RedirectToAction("Index", "Citas", new { area = "Veterinario" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearTratamientoViewModel model)
        {
            int idUsuario = ObtenerIdUsuarioActual();

            if (!ModelState.IsValid)
                return View(model);

            bool ok = await _tratamientoService.CrearTratamientoAsync(
                model.IdMascota,
                model.IdVeterinario,
                model.IdConsulta,
                model.Nombre,
                model.Descripcion,
                model.FechaInicio,
                model.FechaFin
               );

            if (!ok)
            {
                ModelState.AddModelError(string.Empty, "No se pudo crear el tratamiento.");
                return View(model);
            }

            return RedirectToAction("Index", "Dashboard", new { area = "Veterinario" });
        }
    }
}
