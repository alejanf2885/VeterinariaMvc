using System;
using System.Threading.Tasks;
using VeterinariaMvc.Dtos.Tratamiento;
using VeterinariaMvc.Models.Tratamientos;

namespace VeterinariaMvc.Services.Tratamientos
{
    public interface ITratamientoService
    {
        Task<List<TratamientoDto>> GetTratamientosPorMascotaAsync(int idMascota);
        Task<TratamientoDto?> GetTratamientoDetalleAsync(int idTratamiento);
        Task<List<SeguimientoDto>> GetSeguimientosPorTratamientoAsync(int idTratamiento);

        Task<List<TratamientoDto>> GetTratamientosPorUsuarioAsync(int idUsuario); 
        Task<bool> AgregarSeguimientoAsync(int idTratamiento, int idUsuario, string comentario);

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

