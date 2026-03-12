using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VeterinariaMvc.Areas.Cliente.Models;
using VeterinariaMvc.Dtos.Session;
using VeterinariaMvc.Dtos.Tratamiento;
using VeterinariaMvc.Services.Estado;
using VeterinariaMvc.Services.Tratamientos;

namespace VeterinariaMvc.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class TratamientosController : Controller
    {
        private readonly ITratamientoService _tratamientoService;
        private readonly IEstadoUsuarioService _estadoUsuario;

        public TratamientosController(ITratamientoService tratamientoService, IEstadoUsuarioService estadoUsuario)
        {
            _tratamientoService = tratamientoService;
            _estadoUsuario = estadoUsuario;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            UsuarioSessionDto usuario = await _estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            List<TratamientoDto> tratamientosDto = await _tratamientoService.GetTratamientosPorUsuarioAsync(usuario.Id);

            TratamientosViewModel viewModel = new TratamientosViewModel
            {
                Tratamientos = tratamientosDto
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            UsuarioSessionDto usuario = await _estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            TratamientoDto tratamiento = await _tratamientoService.GetTratamientoDetalleAsync(id, usuario.Id);
            if (tratamiento == null)
            {
                TempData["Error"] = "No tienes permiso para ver este tratamiento o no existe.";
                return RedirectToAction(nameof(Index));
            }

            TratamientoDetalleViewModel viewModel = new TratamientoDetalleViewModel
            {
                Tratamiento = tratamiento
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarSeguimiento(int id, string comentario)
        {
            UsuarioSessionDto usuario = await _estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            if (string.IsNullOrWhiteSpace(comentario))
            {
                TempData["Error"] = "El comentario no puede estar vacío.";
                return RedirectToAction(nameof(Detalle), new { id });
            }

            bool resultado = await _tratamientoService.AgregarSeguimientoAsync(id, usuario.Id, comentario);

            TempData[resultado ? "Success" : "Error"] =
                resultado ? "Seguimiento agregado correctamente." : "No se pudo agregar el seguimiento.";

            return RedirectToAction(nameof(Detalle), new { id });
        }
    }
}