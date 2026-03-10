using VeterinariaMvc.Dtos.Bloque;
using VeterinariaMvc.Dtos.Consultas.VeterinariaMvc.Dtos.Consulta;

namespace VeterinariaMvc.Repositories.Consulta
{
    public interface IConsultaRepository
    {
        Task<List<BloqueDisponibleDto>> GetBloquesDisponiblesAsync(int idClinica, DateTime fecha);
        Task<int> ReservarConsultaAsync(int idMascota, int idClinica, int idBloque, string motivo);

        Task<List<ConsultaResumen>> GetConsultasByUserAsync(int idUsuario);
    }
}
