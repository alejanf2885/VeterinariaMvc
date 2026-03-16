using System.Threading.Tasks;
using VeterinariaMvc.Areas.Veterinario.Models;
using VeterinariaMvc.Repositories.Consulta;

namespace VeterinariaMvc.Services.Consulta
{
    public class ConsultaVeterinarioService : IConsultaVeterinarioService
    {
        private readonly IConsultaVeterinarioRepository _repo;

        public ConsultaVeterinarioService(IConsultaVeterinarioRepository repo)
        {
            _repo = repo;
        }

        public async Task<DashboardVeterinarioViewModel> GetDashboardVeterinarioAsync(int idUsuarioVeterinario)
        {
            var citas = await _repo.GetCitasPorVeterinarioAsync(idUsuarioVeterinario);
            return new DashboardVeterinarioViewModel
            {
                CitasAsignadas = citas
            };
        }
    }
}
