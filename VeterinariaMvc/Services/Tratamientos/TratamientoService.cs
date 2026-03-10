using VeterinariaMvc.Dtos.Tratamiento;
using VeterinariaMvc.Repositories.Tratamientos;

namespace VeterinariaMvc.Services.Tratamientos
{
    public class TratamientoService : ITratamientoService
    {
        private readonly ITratamientoRepository _tratamientoRepository;

        public TratamientoService(ITratamientoRepository tratamientoRepository)
        {
            _tratamientoRepository = tratamientoRepository;
        }

        // 1️⃣ Obtener tratamientos por mascota
        // Nota: Eliminé el idUsuario si no lo usas para filtrar aquí, 
        // pero lo mantengo en la firma si tu interfaz lo requiere.
        public async Task<List<TratamientoDto>> GetTratamientosPorMascotaAsync(int idMascota, int idUsuario)
        {
            // El repositorio ya devuelve List<TratamientoDto> con nombres de mascota y veterinario
            return await _tratamientoRepository.GetTratamientosPorMascotaAsync(idMascota);
        }

        // 2️⃣ Obtener tratamientos por usuario
        public async Task<List<TratamientoDto>> GetTratamientosPorUsuarioAsync(int idUsuario)
        {
            return await _tratamientoRepository.GetTratamientosPorUsuarioAsync(idUsuario);
        }

        // 3️⃣ Obtener detalle de un tratamiento
        public async Task<TratamientoDto?> GetTratamientoDetalleAsync(int idTratamiento, int idUsuario)
        {
            // El repositorio ya se encarga de buscar el tratamiento y cargar sus seguimientos
            return await _tratamientoRepository.GetTratamientoDetalleAsync(idTratamiento, idUsuario);
        }

        // 4️⃣ Agregar seguimiento
        public async Task<bool> AgregarSeguimientoAsync(int idTratamiento, int idUsuario, string comentario)
        {
            // Validación de lógica de negocio sencilla
            if (string.IsNullOrWhiteSpace(comentario))
            {
                return false;
            }

            return await _tratamientoRepository.AgregarSeguimientoAsync(idTratamiento, idUsuario, comentario);
        }
    }
}