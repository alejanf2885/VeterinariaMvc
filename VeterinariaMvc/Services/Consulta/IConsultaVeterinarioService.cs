using System.Threading.Tasks;
using VeterinariaMvc.Areas.Veterinario.Models;

namespace VeterinariaMvc.Services.Consulta
{
    public interface IConsultaVeterinarioService
    {
        Task<DashboardVeterinarioViewModel> GetDashboardVeterinarioAsync(int idUsuarioVeterinario);
    }
}
