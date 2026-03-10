using Microsoft.AspNetCore.Mvc;
using MvcCoreSession.Helpers;
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
            UsuarioSessionDto usuario = await this._estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            List<TratamientoDto> tratamientos = await _tratamientoService.GetTratamientosPorUsuarioAsync(usuario.Id);

            var viewModel = new TratamientosViewModel
            {
                Tratamientos = tratamientos
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            UsuarioSessionDto usuario = await this._estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            var tratamiento = await _tratamientoService.GetTratamientoDetalleAsync(id, usuario.Id);

            if (tratamiento == null)
            {
                TempData["Error"] = "No tienes permiso para ver este tratamiento o no existe.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new TratamientoDetalleViewModel
            {
                Tratamiento = tratamiento
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarSeguimiento(int id, string comentario)
        {
            UsuarioSessionDto usuario = await this._estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            if (string.IsNullOrWhiteSpace(comentario))
            {
                TempData["Error"] = "El comentario no puede estar vacío.";
                return RedirectToAction(nameof(Detalle), new { id });
            }

            var resultado = await _tratamientoService.AgregarSeguimientoAsync(id, usuario.Id, comentario);

            if (resultado)
            {
                TempData["Success"] = "Seguimiento agregado correctamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo agregar el seguimiento.";
            }

            return RedirectToAction(nameof(Detalle), new { id });
        }
    }
}
