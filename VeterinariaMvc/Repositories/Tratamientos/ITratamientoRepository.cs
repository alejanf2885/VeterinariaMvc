using VeterinariaMvc.Models.Seguimientos;
using VeterinariaMvc.Models.Tratamientos;

namespace VeterinariaMvc.Repositories.Tratamientos
{
    public interface ITratamientoRepository
    {
        Task<List<TratamientoView>> GetTratamientosPorMascotaAsync(int idMascota);
        Task<List<TratamientoView>> GetTratamientosPorUsuarioAsync(int idUsuario);
        Task<TratamientoView?> GetTratamientoDetalleAsync(int idTratamiento);
        Task<bool> AgregarSeguimientoAsync(int idTratamiento, int idUsuario, string comentario);
        Task<List<SeguimientoView>> GetSeguimientosPorTratamientoAsync(int idTratamiento);

        Task<bool> CrearTratamientoAsync(
            int idMascota,
            int idVeterinario,
            int idConsulta,
            string nombre,
            string? descripcion,
            DateTime fechaInicio,
            DateTime? fechaFin);
    }
}