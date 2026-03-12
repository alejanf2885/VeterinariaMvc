using VeterinariaMvc.Dtos.Tratamiento;
using VeterinariaMvc.Models.Seguimientos;
using VeterinariaMvc.Models.Tratamientos;

namespace VeterinariaMvc.Repositories.Tratamientos
{
    public interface ITratamientoRepository
    {
        Task<List<TratamientoView>> GetTratamientosPorMascotaAsync(int idMascota);
        Task<List<TratamientoView>> GetTratamientosPorUsuarioAsync(int idUsuario);
        Task<TratamientoView?> GetTratamientoDetalleAsync(int idTratamiento, int idUsuario);
        Task<bool> AgregarSeguimientoAsync(int idTratamiento, int idUsuario, string comentario);
        Task<List<SeguimientoView>> GetSeguimientosPorTratamientoAsync(int idTratamiento);
    }
}
