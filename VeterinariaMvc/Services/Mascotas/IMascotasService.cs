using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Dtos.Session;

namespace VeterinariaMvc.Services.Mascotas
{
    public interface IMascotasService
    {
        Task<List<MascotaResumenDto>> GetMascotasByUserAsync(int idUsuario);
        Task<int> RegistrarMascotaAsync(MascotaRegisterDto mascotaRegistrarDto, int idUsuario);
        Task<MascotaDetalleDto?> GetMascotaPorIdAsync(int idMascota, UsuarioSessionDto usuario);
    }
}
