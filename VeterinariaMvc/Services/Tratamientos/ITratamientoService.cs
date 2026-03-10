using VeterinariaMvc.Dtos.Tratamiento;

namespace VeterinariaMvc.Services.Tratamientos
{
    public interface ITratamientoService
    {
        Task<List<TratamientoDto>> GetTratamientosPorMascotaAsync(int idMascota, int idUsuario);
        Task<List<TratamientoDto>> GetTratamientosPorUsuarioAsync(int idUsuario);
        Task<TratamientoDto?> GetTratamientoDetalleAsync(int idTratamiento, int idUsuario);
        Task<bool> AgregarSeguimientoAsync(int idTratamiento, int idUsuario, string comentario);
    }
}
