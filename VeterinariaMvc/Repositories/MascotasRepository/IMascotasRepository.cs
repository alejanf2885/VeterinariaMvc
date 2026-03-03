using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Models;

namespace VeterinariaMvc.Repositories.MascotasRepository
{
    public interface IMascotasRepository
    {
        Task<List<MascotaResumenDto>> GetMascotaPorUsuarioAsync(int idUsuario);

        Task<MascotaDetalle?> GetMascotaPorIdAsync(int idMascota);

        // Permitimos nulos en Especie, Raza, Sexo, Fecha y Peso
        Task<int> RegistrarMascotaAsync(
            string nombre,
            int? idEspecie,
            int? idRaza,
            string? Sexo,
            DateTime? fechaNacimiento,
            double? pesoActual,
            string imagen,
            int idUsuario);

        Task<bool> EliminarMascota(int idMascota);
    }
}