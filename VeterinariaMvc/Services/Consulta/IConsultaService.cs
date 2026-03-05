using VeterinariaMvc.Dtos.Bloque;

namespace VeterinariaMvc.Services.Consulta
{
    public interface IConsultaService
    {
        Task<List<BloqueDisponibleDto>> ObtenerHorariosDisponiblesAsync(int idClinica, DateTime fecha);
        Task<bool> CrearReservaAsync(int idMascota, int idClinica, int idBloque, string motivo);
    }
}
