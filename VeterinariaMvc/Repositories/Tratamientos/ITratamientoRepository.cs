using VeterinariaMvc.Dtos.Tratamiento;
using VeterinariaMvc.Models;

namespace VeterinariaMvc.Repositories.Tratamientos
{
    public interface ITratamientoRepository
    {
        Task<List<TratamientoDto>> GetTratamientosPorMascotaAsync(int idMascota);
        Task<List<TratamientoDto>> GetTratamientosPorUsuarioAsync(int idUsuario);
        Task<TratamientoDto?> GetTratamientoDetalleAsync(int idTratamiento, int idUsuario);
        Task<bool> AgregarSeguimientoAsync(int idTratamiento, int idUsuario, string comentario);
        Task<List<SeguimientoDto>> GetSeguimientosPorTratamientoAsync(int idTratamiento);
    }
}
