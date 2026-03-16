using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VeterinariaMvc.Areas.Cliente.Models;
using VeterinariaMvc.Dtos.Consultas.VeterinariaMvc.Dtos.Consulta;
using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Services.Consulta;
using VeterinariaMvc.Services.Mascotas;

namespace VeterinariaMvc.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    [Authorize]
    public class ConsultasController : Controller
    {
        private readonly IConsultaService _consultaService;
        private readonly IMascotasService _mascotasService;
        private readonly IAuthorizationService _authService;

        public ConsultasController(
            IConsultaService consultaService,
            IMascotasService mascotasService,
            IAuthorizationService authService)
        {
            _consultaService = consultaService;
            _mascotasService = mascotasService;
            _authService = authService;
        }

        private int ObtenerIdUsuarioActual()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }

        [HttpGet]
        public async Task<IActionResult> Reservar(int idMascota, int idClinica, string nombreMascota, string nombreClinica, DateTime? fecha)
        {
            var mascota = await _mascotasService.GetMascotaPorIdAsync(idMascota);
            if (mascota == null) return RedirectToAction("Index", "Home");

            var autorizacion = await _authService.AuthorizeAsync(User, mascota, "PoliticaPermisoMascota");
            if (!autorizacion.Succeeded)
            {
                TempData["ToastMessage"] = "No tienes permiso para solicitar cita para esta mascota.";
                TempData["ToastType"] = "error";
                return RedirectToAction("Index", "Home");
            }

            DateTime fechaBusqueda = fecha ?? DateTime.Today.AddDays(1);

            NuevaReservaViewModel model = new NuevaReservaViewModel
            {
                IdMascota = idMascota,
                IdClinica = idClinica,
                NombreMascota = nombreMascota,
                NombreClinica = nombreClinica,
                FechaSeleccionada = fechaBusqueda,
                HorariosDisponibles = await _consultaService.ObtenerHorariosDisponiblesAsync(idClinica, fechaBusqueda)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarReserva(NuevaReservaViewModel model)
        {
            var mascota = await _mascotasService.GetMascotaPorIdAsync(model.IdMascota);
            if (mascota == null) return RedirectToAction("Index", "Home");

            var autorizacion = await _authService.AuthorizeAsync(User, mascota, "PoliticaPermisoMascota");
            if (!autorizacion.Succeeded)
            {
                TempData["ToastMessage"] = "Intento de reserva no autorizado.";
                TempData["ToastType"] = "error";
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                model.HorariosDisponibles = await _consultaService.ObtenerHorariosDisponiblesAsync(model.IdClinica, model.FechaSeleccionada);
                return View("Reservar", model);
            }

            bool exito = await _consultaService.CrearReservaAsync(model.IdMascota, model.IdClinica, model.IdBloque.Value, model.Motivo);

            if (exito)
            {
                TempData["ToastMessage"] = "¡Cita reservada correctamente!";
                TempData["ToastType"] = "success";
                return RedirectToAction("Detalles", "Mascotas", new { id = model.IdMascota });
            }

            ViewData["ERROR"] = "El horario seleccionado ya no está disponible. Por favor, elige otro.";
            model.HorariosDisponibles = await _consultaService.ObtenerHorariosDisponiblesAsync(model.IdClinica, model.FechaSeleccionada);
            return View("Reservar", model);
        }

        public async Task<IActionResult> Consultas()
        {
            int idUsuario = ObtenerIdUsuarioActual();
            List<ConsultaResumen> consultas = await _consultaService.GetHistorialCompletoAsync(idUsuario);

            return View(consultas);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            ConsultaResumen consulta = await _consultaService.GetConsultaDetalleAsync(id);

            if (consulta == null)
            {
                TempData["ToastMessage"] = "La consulta no existe.";
                TempData["ToastType"] = "error";
                return RedirectToAction("Consultas");
            }

            var autorizacion = await _authService.AuthorizeAsync(User, consulta, "PoliticaPermisoConsulta");
            if (!autorizacion.Succeeded)
            {
                TempData["ToastMessage"] = "No tienes permiso para ver esta consulta.";
                TempData["ToastType"] = "error";
                return RedirectToAction("Consultas");
            }

            return View(consulta);
        }

        [HttpPost] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancelar(int id)
        {
            ConsultaResumen consulta = await _consultaService.GetConsultaDetalleAsync(id);

            if (consulta == null) return RedirectToAction("Consultas");

            var autorizacion = await _authService.AuthorizeAsync(User, consulta, "PoliticaPermisoConsulta");
            if (!autorizacion.Succeeded)
            {
                TempData["ToastMessage"] = "Intento de cancelación no autorizado.";
                TempData["ToastType"] = "error";
                return RedirectToAction("Consultas");
            }

            bool exito = await _consultaService.CancelarConsultaAsync(id);

            if (exito)
            {
                TempData["ToastMessage"] = "Consulta cancelada correctamente.";
                TempData["ToastType"] = "success";
            }
            else
            {
                TempData["ToastMessage"] = "No se pudo cancelar la consulta. Intenta de nuevo.";
                TempData["ToastType"] = "error";
            }

            return RedirectToAction("Consultas");
        }
    }
}