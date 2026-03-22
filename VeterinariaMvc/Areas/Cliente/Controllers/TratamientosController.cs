using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeterinariaMvc.Areas.Cliente.Models;
using VeterinariaMvc.Dtos.Tratamiento;
using VeterinariaMvc.Services.Tratamientos;
using VeterinariaMvc.Services.Mascotas;
using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Services.Estado;

namespace VeterinariaMvc.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    [Authorize]
    public class TratamientosController : Controller
    {
        private readonly ITratamientoService _tratamientoService;
        private readonly IMascotasService _mascotasService;
        private readonly IAuthorizationService _authService;
        private readonly IEstadoUsuarioService _estadoUsuarioService;

        public TratamientosController(
            ITratamientoService tratamientoService,
            IMascotasService mascotasService,
            IAuthorizationService authService,
            IEstadoUsuarioService estadoUsuarioService)
        {
            _tratamientoService = tratamientoService;
            _mascotasService = mascotasService;
            _authService = authService;
            _estadoUsuarioService = estadoUsuarioService;
        }

        private async Task<int> ObtenerIdUsuarioActualAsync()
        {
            var usuario = await _estadoUsuarioService.ObtenerUsuarioActualAsync();
            return usuario?.Id ?? 0;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int idUsuario = await ObtenerIdUsuarioActualAsync();

            List<TratamientoDto> tratamientosDto = await _tratamientoService.GetTratamientosPorUsuarioAsync(idUsuario);

            TratamientosViewModel viewModel = new TratamientosViewModel
            {
                Tratamientos = tratamientosDto
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            TratamientoDto? tratamiento = await _tratamientoService.GetTratamientoDetalleAsync(id);

            if (tratamiento == null)
            {
                TempData["Error"] = "El tratamiento no existe.";
                return RedirectToAction(nameof(Index));
            }

            var autorizacion = await _authService.AuthorizeAsync(User, tratamiento, "PoliticaPermisoTratamiento");
            if (!autorizacion.Succeeded)
            {
                TempData["Error"] = "No tienes permiso para ver este tratamiento.";
                return RedirectToAction(nameof(Index));
            }

            TratamientoDetalleViewModel viewModel = new TratamientoDetalleViewModel
            {
                Tratamiento = tratamiento
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarSeguimiento(int id, string comentario)
        {
            if (string.IsNullOrWhiteSpace(comentario))
            {
                TempData["Error"] = "El comentario no puede estar vacío.";
                return RedirectToAction(nameof(Detalle), new { id });
            }

            TratamientoDto? tratamiento = await _tratamientoService.GetTratamientoDetalleAsync(id);
            if (tratamiento == null) return RedirectToAction(nameof(Index));

            var autorizacion = await _authService.AuthorizeAsync(User, tratamiento, "PoliticaPermisoTratamiento");
            if (!autorizacion.Succeeded)
            {
                TempData["Error"] = "No tienes permiso para comentar en este tratamiento.";
                return RedirectToAction(nameof(Index));
            }

            int idUsuario = await ObtenerIdUsuarioActualAsync();
            bool resultado = await _tratamientoService.AgregarSeguimientoAsync(id, idUsuario, comentario);

            TempData[resultado ? "Success" : "Error"] =
                resultado ? "Seguimiento agregado correctamente." : "No se pudo agregar el seguimiento.";

            return RedirectToAction(nameof(Detalle), new { id });
        }

        public async Task<IActionResult> TratamientosMascota(int idmascota)
        {
            MascotaDetalleDto? mascota = await _mascotasService.GetMascotaPorIdAsync(idmascota);
            if (mascota == null) return RedirectToAction("Index", "Home");

            var autorizacion = await _authService.AuthorizeAsync(User, mascota, "PoliticaPermisoMascota");
            if (!autorizacion.Succeeded)
            {
                TempData["Error"] = "No tienes permiso para ver los tratamientos de esta mascota.";
                return RedirectToAction("Index", "Home");
            }

            List<TratamientoDto> tratamientos = await _tratamientoService.GetTratamientosPorMascotaAsync(idmascota);

            return View(tratamientos);
        }
    }
}