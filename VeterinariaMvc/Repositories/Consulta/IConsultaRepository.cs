using VeterinariaMvc.Dtos.Bloque;

namespace VeterinariaMvc.Repositories.Consulta
{
    public interface IConsultaRepository
    {
        Task<List<BloqueDisponibleDto>> GetBloquesDisponiblesAsync(int idClinica, DateTime fecha);
        Task<int> ReservarConsultaAsync(int idMascota, int idClinica, int idBloque, string motivo);
    }
}
