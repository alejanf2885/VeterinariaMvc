using VeterinariaMvc.Dtos.Bloque;
using VeterinariaMvc.Dtos.Consultas.VeterinariaMvc.Dtos.Consulta;

namespace VeterinariaMvc.Repositories.Consulta
{
    public interface IConsultaRepository
    {
        Task<List<BloqueDisponibleDto>> GetBloquesDisponiblesAsync(int idClinica, DateTime fecha);
        Task<int> ReservarConsultaAsync(int idMascota, int idClinica, int idBloque, string motivo);

        Task<List<ConsultaResumen>> GetConsultasByUserAsync(int idUsuario);

        Task<ConsultaResumen> GetConsultaDetalleAsync(int idConsulta);

        Task<bool> CancelarConsultaAsync(int idConsulta);

        // Dashboard para clínica (admin)
        Task<List<DashboardCitaSinVeterinario>> GetCitasSinVeterinarioAsync(int idClinica);
        Task<List<DashboardCitaVeterinario>> GetCitasPorVeterinarioAsync(int idClinica);

        Task<bool> AsignarVeterinarioAsync(int idConsulta, int idVeterinario);
        Task<bool> ActualizarEstadoConsultaAsync(int idConsulta, string estado);
    }
}
