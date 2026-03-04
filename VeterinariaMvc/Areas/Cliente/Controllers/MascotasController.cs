using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VeterinariaMvc.Areas.Cliente.Models;
using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Dtos.Session;
using VeterinariaMvc.Services.Estado;
using VeterinariaMvc.Services.MascotaCatalogosService;
using VeterinariaMvc.Services.Mascotas;

namespace VeterinariaMvc.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class MascotasController : Controller
    {
        private IEstadoUsuarioService _estadoUsuario;
        private IMascotasService _mascotasService;
        private IMascotaCatalogoService _mascotaCatalogoService;

        public MascotasController(IEstadoUsuarioService estadoUsuario, IMascotasService mascotasService, IMascotaCatalogoService mascotaCatalogoService)
        {
            this._estadoUsuario = estadoUsuario;
            this._mascotasService = mascotasService;
            this._mascotaCatalogoService = mascotaCatalogoService;
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
            UsuarioSessionDto usuario = await this._estadoUsuario.ObtenerUsuarioActualAsync();

            if (usuario == null) return RedirectToAction("Login", "Auth", new { area = "" });

            int mascotaId = await this._mascotasService.RegistrarMascotaAsync(model.Formulario, usuario.Id);




            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Detalles(int id)
        {
            UsuarioSessionDto usuario = await this._estadoUsuario.ObtenerUsuarioActualAsync();

            if (usuario == null) return RedirectToAction("Login", "Auth", new { area = "" });

            MascotaDetalleDto mascotaDetalleDto = await this._mascotasService.GetMascotaPorIdAsync(id, usuario);

            if (mascotaDetalleDto == null) return RedirectToAction("Index", "Home");

            return View(mascotaDetalleDto);
        }

        public async Task<IActionResult> Editar(int id)
        {
            UsuarioSessionDto usuario = await this._estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null) return RedirectToAction("Login", "Auth", new { area = "" });

            MascotaEditDto? mascotaEditDto = await this._mascotasService.GetMascotaParaEditarAsync(id, usuario);
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

            bool ok = await this._mascotasService.EditarMascotaAsync(model.Formulario, usuario);
            if (!ok) return RedirectToAction("Index", "Home");

            return RedirectToAction("Detalles", new { id });
        }


        [HttpPost]
        public async Task<IActionResult> Desactivar(int idMascota)
        {
            UsuarioSessionDto usuario = await this._estadoUsuario.ObtenerUsuarioActualAsync();
            if (usuario == null) return RedirectToAction("Login", "Auth", new { area = "" });

            bool ok = await this._mascotasService.DesactivarMascotaAsync(idMascota, usuario);

            // Preparamos el dashboard igual que en HomeController.Index
            List<MascotaResumenDto> mascotas = await _mascotasService.GetMascotasByUserAsync(usuario.Id);
            DashboardViewModel model = new DashboardViewModel
            {
                usuario = usuario,
                Mascotas = mascotas
            };

            // Usamos ViewData para el toast
            ViewData["ToastMessage"] = ok
                ? "La mascota ha sido desactivada correctamente."
                : "No se ha podido desactivar la mascota.";
            ViewData["ToastType"] = ok ? "success" : "error";

            // Renderizamos directamente la vista de panel del área Cliente
            return View("~/Areas/Cliente/Views/Home/Index.cshtml", model);
        }



    }
}
