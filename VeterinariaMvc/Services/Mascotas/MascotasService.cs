using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Repositories.MascotasRepository;

namespace VeterinariaMvc.Services.Mascotas
{
    public class MascotasService : IMascotasService
    {

        private IMascotasRepository _mascotasRepo;

        public MascotasService(IMascotasRepository mascotasRepo)
        {
            this._mascotasRepo = mascotasRepo;
        }

        public async Task<List<MascotaResumenDto>> GetMascotasByUserAsync(int idUsuario)
        {

            List<MascotaResumenDto> mascotas = 
                await this._mascotasRepo.GetMascotaPorUsuario(idUsuario);

            return mascotas;
        }
    }
}
