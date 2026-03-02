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

        public async Task<List<MascotaResumenDto>> GetMascotaPorUsuario(int idUsuario)
        {

            string sql = "EXEC SP_OBTENERMASCOTASBYUSUARIO @IdUsuario";

            SqlParameter pamIdUsuario = new SqlParameter("@IdUsuario", idUsuario);

            List<MascotaResumenDto> mascotas = await
                this._context.Database.SqlQueryRaw<MascotaResumenDto>(sql, pamIdUsuario).ToListAsync();

            return mascotas;

        }
    }
}
