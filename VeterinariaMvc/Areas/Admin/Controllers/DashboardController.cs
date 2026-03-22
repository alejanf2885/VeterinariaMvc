using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VeterinariaMvc.Areas.Admin.Models;
using VeterinariaMvc.Services.Consulta;
using VeterinariaMvc.Services.Veterinarios;
using VeterinariaMvc.Services.Mascotas;
using VeterinariaMvc.Services.Estado;

namespace VeterinariaMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "1")]
    public class DashboardController : Controller
    {
        private readonly IConsultaService _consultaService;
        private readonly IVeterinarioService _veterinarioService;
        private readonly IMascotasService _mascotasService;
        private readonly IEstadoUsuarioService _estadoUsuarioService;

        public DashboardController(
            IConsultaService consultaService,
            IVeterinarioService veterinarioService,
            IMascotasService mascotasService,
            IEstadoUsuarioService estadoUsuarioService)
        {
            _consultaService = consultaService;
            _veterinarioService = veterinarioService;
            _mascotasService = mascotasService;
            _estadoUsuarioService = estadoUsuarioService;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = await _estadoUsuarioService.ObtenerUsuarioActualAsync();

            if (usuario == null || usuario.IdClinica == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            int idClinica = usuario.IdClinica.Value;

            var modelo = new DashboardVeterinariosViewModel
            {
                CitasSinVeterinario = await _consultaService
                    .GetCitasSinVeterinarioDashboardAsync(idClinica),

                CitasPorVeterinario = await _consultaService
                    .GetCitasPorVeterinarioDashboardAsync(idClinica)
            };

            modelo.TotalCitasSinVeterinario = modelo.CitasSinVeterinario.Count;

            modelo.TotalConsultasHoy = modelo.CitasPorVeterinario
                .Count(c => c.FechaHoraConsulta.Date == DateTime.Today);

            modelo.TotalMascotasRegistradas = await _mascotasService
                .ObtenerTotalMascotasPorClinicaAsync(idClinica);

            ViewBag.Veterinarios = await _veterinarioService
                .ObtenerVeterinariosPorClinicaAsync(idClinica);

            return View(modelo);
        }
    }
}