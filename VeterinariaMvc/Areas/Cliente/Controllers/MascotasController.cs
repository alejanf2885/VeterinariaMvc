using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeterinariaMvc.Areas.Cliente.Models;
using VeterinariaMvc.Dtos.Clinica;
using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Dtos.Session;
using VeterinariaMvc.Services.Clinica;
using VeterinariaMvc.Services.Estado;
using VeterinariaMvc.Services.MascotaCatalogosService;
using VeterinariaMvc.Services.Mascotas;
using VeterinariaMvc.Services.Tratamientos;

namespace VeterinariaMvc.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    [Authorize]
    public class MascotasController : Controller
    {
        private readonly IEstadoUsuarioService _estadoUsuario;
        private readonly IMascotasService _mascotasService;
        private readonly IMascotaCatalogoService _mascotaCatalogoService;
        private readonly IClinicaService _clinicaService;
        private readonly ITratamientoService _tratamientoService;
        private readonly IAuthorizationService _authService;

        public MascotasController(
            IEstadoUsuarioService estadoUsuario,
            IMascotasService mascotasService,
            IMascotaCatalogoService mascotaCatalogoService,
            IClinicaService clinicaService,
            ITratamientoService tratamientoService,
            IAuthorizationService authService)
        {
            _estadoUsuario = estadoUsuario;
            _mascotasService = mascotasService;
            _mascotaCatalogoService = mascotaCatalogoService;
            _clinicaService = clinicaService;
            _tratamientoService = tratamientoService;
            _authService = authService;
        }

        public async Task<IActionResult> Registrar()
        {
            CatalogosMascotaViewModels catalogo = await _mascotaCatalogoService.GetCatalogoMascotasAsync();
            var model = new RegistrarMascotaViewModel
            {
                Catalogos = catalogo,
                Formulario = new MascotaRegisterDto()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(RegistrarMascotaViewModel model)
        {
            var usuario = await _estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null) return RedirectToAction("Login", "Auth", new { area = "" });

            await _mascotasService.RegistrarMascotaAsync(model.Formulario, usuario.Id);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Detalles(int id)
        {
            var mascotaDetalleDto = await _mascotasService.GetMascotaPorIdAsync(id);
            if (mascotaDetalleDto == null) return RedirectToAction("Index", "Home");

            var autorizacion = await _authService.AuthorizeAsync(User, mascotaDetalleDto, "PoliticaPermisoMascota");
            if (!autorizacion.Succeeded)
            {
                TempData["ToastMessage"] = "No tienes permiso para ver los detalles de esta mascota.";
                TempData["ToastType"] = "error";
                return RedirectToAction("Index", "Home");
            }

            var model = new MascotaDetalleViewModel
            {
                Mascota = mascotaDetalleDto,
                Clinicas = await _clinicaService.GetClinicasAsync() ?? new List<ClinicaDto>(),
                Tratamientos = await _tratamientoService.GetTratamientosPorMascotaAsync(id)
            };
            return View(model);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var usuario = await _estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null) return RedirectToAction("Login", "Auth", new { area = "" });

            var mascotaEditDto = await _mascotasService.GetMascotaParaEditarAsync(id);
            if (mascotaEditDto == null) return RedirectToAction("Index", "Home");

            var catalogos = await _mascotaCatalogoService.GetCatalogoMascotasAsync();
            var model = new EditarMascotaViewModel
            {
                Formulario = mascotaEditDto,
                Catalogos = catalogos
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, EditarMascotaViewModel model)
        {
            var usuario = await _estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null) return RedirectToAction("Login", "Auth", new { area = "" });

            if (!ModelState.IsValid)
            {
                model.Catalogos = await _mascotaCatalogoService.GetCatalogoMascotasAsync();
                return View(model);
            }

            model.Formulario.Id = id;
            bool ok = await _mascotasService.EditarMascotaAsync(model.Formulario);
            if (!ok) return RedirectToAction("Index", "Home");

            return RedirectToAction("Detalles", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Desactivar(int idMascota)
        {
            var usuario = await _estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null) return RedirectToAction("Login", "Auth", new { area = "" });

            bool ok = await _mascotasService.DesactivarMascotaAsync(idMascota);

            TempData["ToastMessage"] = ok
                ? "La mascota ha sido desactivada correctamente."
                : "No se ha podido desactivar la mascota.";
            TempData["ToastType"] = ok ? "success" : "error";

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> AsignarClinica(int idMascota, int idClinica)
        {
            var usuario = await _estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null) return RedirectToAction("Login", "Auth", new { area = "" });

            bool ok = await _mascotasService.AsignarClinicaAMascotaAsync(idMascota, idClinica);

            TempData["ToastMessage"] = ok
                ? "La clínica ha sido asignada correctamente."
                : "No se ha podido asignar la clínica. Verifica que la mascota te pertenezca.";
            TempData["ToastType"] = ok ? "success" : "error";

            return RedirectToAction("Detalles", new { id = idMascota });
        }
    }
}