using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Models;

namespace VeterinariaMvc.Repositories.MascotasRepository
{
    public interface IMascotasRepository
    {

        Task<List<MascotaResumenDto>> GetMascotaPorUsuario(int idUsuario);


    }
}
