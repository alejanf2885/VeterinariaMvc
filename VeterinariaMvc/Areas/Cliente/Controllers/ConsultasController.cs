using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeterinariaMvc.Areas.Cliente.Models;
using VeterinariaMvc.Dtos.Consultas.VeterinariaMvc.Dtos.Consulta;
using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Dtos.Session;
using VeterinariaMvc.Services.Consulta;
using VeterinariaMvc.Services.Estado;
using VeterinariaMvc.Services.Mascotas;

namespace VeterinariaMvc.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class ConsultasController : Controller
    {
        private readonly IConsultaService _consultaService;
        private readonly IMascotasService _mascotasService;
        private readonly IEstadoUsuarioService _estadoUsuario;

        public ConsultasController(
            IConsultaService consultaService,
            IMascotasService mascotasService,
            IEstadoUsuarioService estadoUsuario)
        {
            _consultaService = consultaService;
            _mascotasService = mascotasService;
            _estadoUsuario = estadoUsuario;
        }

        [HttpGet]
        public async Task<IActionResult> Reservar(int idMascota, int idClinica, string nombreMascota, string nombreClinica, DateTime? fecha)
        {
            // 1. Obtener usuario actual
            UsuarioSessionDto usuario = await _estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null) return RedirectToAction("Login", "Auth", new { area = "" });

            // 2. SEGURIDAD: Verificar que la mascota le pertenece al usuario.
            // Usamos tu servicio que ya tiene esta lógica integrada.
            MascotaDetalleDto mascota = await _mascotasService.GetMascotaPorIdAsync(idMascota, usuario);
            if (mascota == null)
            {
                // Si la mascota no es suya (o no existe), lo echamos de aquí
                TempData["ToastMessage"] = "No tienes permiso para solicitar cita para esta mascota.";
                TempData["ToastType"] = "error";
                return RedirectToAction("Index", "Home");
            }

            // 3. Preparar el modelo
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
            // 1. Obtener usuario actual
            UsuarioSessionDto usuario = await _estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null) return RedirectToAction("Login", "Auth", new { area = "" });

            // 2. SEGURIDAD: Verificar que la mascota le pertenece al usuario.
            MascotaDetalleDto mascota = await _mascotasService.GetMascotaPorIdAsync(model.IdMascota, usuario);
            if (mascota == null)
            {
                TempData["ToastMessage"] = "Intento de reserva no autorizado.";
                TempData["ToastType"] = "error";
                return RedirectToAction("Index", "Home");
            }

            // 3. Validar formulario
            if (!ModelState.IsValid)
            {
                model.HorariosDisponibles = await _consultaService.ObtenerHorariosDisponiblesAsync(model.IdClinica, model.FechaSeleccionada);
                return View("Reservar", model);
            }

            // 4. Guardar Reserva
            bool exito = await _consultaService.CrearReservaAsync(model.IdMascota, model.IdClinica, model.IdBloque.Value, model.Motivo);

            if (exito)
            {
                TempData["ToastMessage"] = "¡Cita reservada correctamente!";
                TempData["ToastType"] = "success";
                return RedirectToAction("Detalles", "Mascotas", new { id = model.IdMascota });
            }

            // 5. Si falló la BD (ej. el turno se ocupó en el último segundo)
            ViewData["ERROR"] = "El horario seleccionado ya no está disponible. Por favor, elige otro.";
            model.HorariosDisponibles = await _consultaService.ObtenerHorariosDisponiblesAsync(model.IdClinica, model.FechaSeleccionada);
            return View("Reservar", model);
        }


        public async Task<IActionResult> Consultas()
        {
            // 1. Obtener usuario actual
            UsuarioSessionDto usuario = await _estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null) return RedirectToAction("Login", "Auth", new { area = "" });
            // 2. Obtener consultas del usuario
            List<ConsultaResumen> consultas = await _consultaService.GetHistorialCompletoAsync(usuario.Id);
            return View(consultas);
        }
    }
}