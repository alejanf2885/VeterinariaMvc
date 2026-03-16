using VeterinariaMvc.Dtos.Bloque;
using VeterinariaMvc.Dtos.Consultas.VeterinariaMvc.Dtos.Consulta;

namespace VeterinariaMvc.Services.Consulta
{
    public interface IConsultaService
    {
        Task<List<BloqueDisponibleDto>> ObtenerHorariosDisponiblesAsync(int idClinica, DateTime fecha);
        Task<bool> CrearReservaAsync(int idMascota, int idClinica, int idBloque, string motivo);
        Task<List<ConsultaResumen>> GetConsultasDashboardAsync(int idUsuario);
        Task<List<ConsultaResumen>> GetHistorialCompletoAsync(int idUsuario);
        Task<ConsultaResumen> GetConsultaDetalleAsync(int idConsulta);
        Task<bool> CancelarConsultaAsync(int idConsulta);

        // Dashboard para clínica (admin)
        Task<List<DashboardCitaSinVeterinario>> GetCitasSinVeterinarioDashboardAsync(int idClinica);
        Task<List<DashboardCitaVeterinario>> GetCitasPorVeterinarioDashboardAsync(int idClinica);
        Task<bool> AsignarVeterinarioAsync(int idConsulta, int idVeterinario);
    }
}