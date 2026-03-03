using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Models;

namespace VeterinariaMvc.Repositories.MascotasRepository
{
    public interface IMascotasRepository
    {

        Task<List<MascotaResumenDto>> GetMascotaPorUsuario(int idUsuario);

        Task<Mascota?> GetMascotaPorId(int idMascota);

            Task<bool> RegistrarMascota(MascotaRegisterDto mascotaRegistrarDto, int idUsuario);
    
            Task<bool> EliminarMascota(int idMascota);


    }
}
