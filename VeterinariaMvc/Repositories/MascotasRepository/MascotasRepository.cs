using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Data;
using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Models;

namespace VeterinariaMvc.Repositories.MascotasRepository
{
    public class MascotasRepository : IMascotasRepository
    {
        private Context _context;

        public MascotasRepository(Context context)
        {
            this._context = context;
        }

        public Task<bool> EliminarMascota(int idMascota)
        {
            throw new NotImplementedException();
        }

        public Task<Mascota?> GetMascotaPorId(int idMascota)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MascotaResumenDto>> GetMascotaPorUsuario(int idUsuario)
        {

            string sql = "EXEC SP_OBTENERMASCOTASBYUSUARIO @IdUsuario";

            SqlParameter pamIdUsuario = new SqlParameter("@IdUsuario", idUsuario);

            List<MascotaResumenDto> mascotas = await
                this._context.Database.SqlQueryRaw<MascotaResumenDto>(sql, pamIdUsuario).ToListAsync();

            return mascotas;

        }

        public Task<bool> RegistrarMascota(MascotaRegisterDto mascotaRegistrarDto, int idUsuario)
        {
            throw new NotImplementedException();
        }
    }
}
