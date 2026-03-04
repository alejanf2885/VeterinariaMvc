using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Dtos.Session;

namespace VeterinariaMvc.Services.Mascotas
{
    public interface IMascotasService
    {
        Task<List<MascotaResumenDto>> GetMascotasByUserAsync(int idUsuario);
        Task<int> RegistrarMascotaAsync(MascotaRegisterDto mascotaRegistrarDto, int idUsuario);
        Task<MascotaDetalleDto?> GetMascotaPorIdAsync(int idMascota, UsuarioSessionDto usuario);

        Task<MascotaEditDto?> GetMascotaParaEditarAsync(int idMascota, UsuarioSessionDto usuario);
        Task<bool> EditarMascotaAsync(MascotaEditDto dto, UsuarioSessionDto usuario);
        Task<bool> DesactivarMascotaAsync(int idMascota, UsuarioSessionDto usuario);
        Task<bool> AsignarClinicaAMascotaAsync(int idMascota, int idClinica, UsuarioSessionDto usuario);
    }
}
