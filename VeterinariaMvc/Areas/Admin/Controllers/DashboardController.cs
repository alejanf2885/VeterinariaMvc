using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VeterinariaMvc.Areas.Admin.Models;
using VeterinariaMvc.Services.Consulta;
using VeterinariaMvc.Services.Veterinarios;
using VeterinariaMvc.Services.Mascotas;

namespace VeterinariaMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IConsultaService _consultaService;
        private readonly IVeterinarioService _veterinarioService;
        private readonly IMascotasService _mascotasService;

        public DashboardController(IConsultaService consultaService, IVeterinarioService veterinarioService, IMascotasService mascotasService)
        {
            _consultaService = consultaService;
            _veterinarioService = veterinarioService;
            _mascotasService = mascotasService;
        }

        public async Task<IActionResult> Index()
        {
            int idClinica = int.Parse(User.FindFirst("IdClinica")?.Value ?? "0");

            var modelo = new DashboardVeterinariosViewModel
            {
                CitasSinVeterinario = await _consultaService.GetCitasSinVeterinarioDashboardAsync(idClinica),
                CitasPorVeterinario = await _consultaService.GetCitasPorVeterinarioDashboardAsync(idClinica)
            };

            // Estadísticas
            modelo.TotalCitasSinVeterinario = modelo.CitasSinVeterinario.Count;
            modelo.TotalConsultasHoy = modelo.CitasPorVeterinario
                .Count(c => c.FechaHoraConsulta.Date == DateTime.Today);

            // Total de mascotas registradas en esta clínica
            modelo.TotalMascotasRegistradas = await _mascotasService.ObtenerTotalMascotasPorClinicaAsync(idClinica);

            // Veterinarios disponibles en esta clínica para asignar a las citas
            ViewBag.Veterinarios = await _veterinarioService.ObtenerVeterinariosPorClinicaAsync(idClinica);

            return View(modelo);
        }
    }
}
