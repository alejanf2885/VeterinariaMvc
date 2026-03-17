using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Models;

namespace VeterinariaMvc.Services.Mascotas
{
    public interface IMascotasService
    {
        Task<List<MascotaResumenDto>> GetMascotasByUserAsync(int idUsuario);
        Task<int> RegistrarMascotaAsync(MascotaRegisterDto mascotaRegistrarDto, int idUsuario);

        Task<MascotaDetalleDto?> GetMascotaPorIdAsync(int idMascota);
        Task<MascotaEditDto?> GetMascotaParaEditarAsync(int idMascota);
        Task<bool> EditarMascotaAsync(MascotaEditDto dto);
        Task<bool> DesactivarMascotaAsync(int idMascota);
        Task<bool> AsignarClinicaAMascotaAsync(int idMascota, int idClinica);
        Task<List<MascotaDetalle>> ObtenerMascotasPorClinicaAsync(int idClinica);
        Task<int> ObtenerTotalMascotasPorClinicaAsync(int idClinica);
    }
}