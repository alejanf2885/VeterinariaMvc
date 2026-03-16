using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        private IEstadoUsuarioService _estadoUsuario;
        private IMascotasService _mascotasService;
        private IMascotaCatalogoService _mascotaCatalogoService;
        private IClinicaService _clinicaService;
        private ITratamientoService _tratamientoService;
        private readonly IAuthorizationService _authService;

        public MascotasController(
            IEstadoUsuarioService estadoUsuario, 
            IMascotasService mascotasService, 
            IMascotaCatalogoService mascotaCatalogoService, 
            IClinicaService clinicaService,
            ITratamientoService tratamientoService,
            IAuthorizationService authService)
        {
            this._estadoUsuario = estadoUsuario;
            this._mascotasService = mascotasService;
            this._mascotaCatalogoService = mascotaCatalogoService;
            this._clinicaService = clinicaService;
            this._tratamientoService = tratamientoService;
            this._authService = authService;
        }

        public async Task<IActionResult> Registrar()
        {
            CatalogosMascotaViewModels catalogo =
                await this._mascotaCatalogoService.GetCatalogoMascotasAsync();

            RegistrarMascotaViewModel registrarMascotaViewModel = new RegistrarMascotaViewModel();

            registrarMascotaViewModel.Catalogos = catalogo;
            registrarMascotaViewModel.Formulario = new MascotaRegisterDto();


            return View(registrarMascotaViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Registrar(RegistrarMascotaViewModel model)
        {
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            await _mascotasService.RegistrarMascotaAsync(model.Formulario, idUsuario);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Detalles(int id)
        {
            MascotaDetalleDto? mascotaDetalleDto = await _mascotasService.GetMascotaPorIdAsync(id);
            if (mascotaDetalleDto == null) return RedirectToAction("Index", "Home");

            var autorizacion = await _authService.AuthorizeAsync(User, mascotaDetalleDto, "PoliticaPermisoMascota");
            if (!autorizacion.Succeeded)
            {
                TempData["ToastMessage"] = "No tienes permiso para ver los detalles de esta mascota.";
                TempData["ToastType"] = "error";
                return RedirectToAction("Index", "Home");
            }

            MascotaDetalleViewModel model = new MascotaDetalleViewModel
            {
                Mascota = mascotaDetalleDto,
                Clinicas = await _clinicaService.GetClinicasAsync() ?? new List<ClinicaDto>(),
                Tratamientos = await _tratamientoService.GetTratamientosPorMascotaAsync(id)
            };

            return View(model);
        }

        public async Task<IActionResult> Editar(int id)
        {
            UsuarioSessionDto usuario = await this._estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null) return RedirectToAction("Login", "Auth", new { area = "" });

            MascotaEditDto? mascotaEditDto = await this._mascotasService.GetMascotaParaEditarAsync(id);
            if (mascotaEditDto == null) return RedirectToAction("Index", "Home");

            CatalogosMascotaViewModels catalogos =
                await this._mascotaCatalogoService.GetCatalogoMascotasAsync();

            EditarMascotaViewModel vm = new EditarMascotaViewModel();
            vm.Formulario = mascotaEditDto;
            vm.Catalogos = catalogos;


            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, EditarMascotaViewModel model)
        {
            UsuarioSessionDto usuario = await this._estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null) return RedirectToAction("Login", "Auth", new { area = "" });

            if (!ModelState.IsValid)
            {
                model.Catalogos = await this._mascotaCatalogoService.GetCatalogoMascotasAsync();
                return View(model);
            }

            model.Formulario.Id = id;

            bool ok = await this._mascotasService.EditarMascotaAsync(model.Formulario);
            if (!ok) return RedirectToAction("Index", "Home");

            return RedirectToAction("Detalles", new { id });
        }


        [HttpPost]
        public async Task<IActionResult> Desactivar(int idMascota)
        {
            UsuarioSessionDto usuario = await this._estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null) return RedirectToAction("Login", "Auth", new { area = "" });

            bool ok = await this._mascotasService.DesactivarMascotaAsync(idMascota);

           

            TempData["ToastMessage"] = ok
                ? "La mascota ha sido desactivada correctamente."
                : "No se ha podido desactivar la mascota.";
            TempData["ToastType"] = ok ? "success" : "error";

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> AsignarClinica(int idMascota, int idClinica)
        {
            UsuarioSessionDto usuario = await this._estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null) return RedirectToAction("Login", "Auth", new { area = "" });

            bool ok = await this._mascotasService.AsignarClinicaAMascotaAsync(idMascota, idClinica);

            TempData["ToastMessage"] = ok
                ? "La clínica ha sido asignada correctamente."
                : "No se ha podido asignar la clínica. Verifica que la mascota te pertenezca.";
            TempData["ToastType"] = ok ? "success" : "error";

            return RedirectToAction("Detalles", new { id = idMascota });
        }

    }
}
