using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
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

        public async Task<MascotaDetalle?> GetMascotaPorIdAsync(int idMascota)
        {

            var consulta = from datos in this._context.MascotasDetalles
                           where datos.IdMascota == idMascota
                           select datos;

            MascotaDetalle mascota = await consulta.FirstOrDefaultAsync();

            return mascota;
        }

        public async Task<List<MascotaResumenDto>> GetMascotaPorUsuarioAsync(int idUsuario)
        {

            string sql = "EXEC SP_OBTENERMASCOTASBYUSUARIO @IdUsuario";

            SqlParameter pamIdUsuario = new SqlParameter("@IdUsuario", idUsuario);

            List<MascotaResumenDto> mascotas = await
                this._context.Database.SqlQueryRaw<MascotaResumenDto>(sql, pamIdUsuario).ToListAsync();

            return mascotas;

        }
        public async Task<int> RegistrarMascotaAsync(string nombre, int? idEspecie, int? idRaza, string? Sexo, DateTime? fechaNacimiento, double? pesoActual, string imagen, int idUsuario)
        {
            string sql = "EXEC SP_INSERTARMASCOTA @IdUsuario, @Nombre, @Sexo, @FechaNacimiento, @PesoActual, @Imagen, @IdEspecie, @IdRaza, @@NuevoId OUTPUT";

            SqlParameter pamIdUsuario = new SqlParameter("@IdUsuario", idUsuario);
            SqlParameter pamNombre = new SqlParameter("@Nombre", nombre);
            SqlParameter pamSexo = new SqlParameter("@Sexo", (object)Sexo ?? DBNull.Value);
            SqlParameter pamFechaNacimiento = new SqlParameter("@FechaNacimiento", (object)fechaNacimiento ?? DBNull.Value);
            SqlParameter pamPesoActual = new SqlParameter("@PesoActual", (object)pesoActual ?? DBNull.Value);
            SqlParameter pamImagen = new SqlParameter("@Imagen", imagen);
            SqlParameter pamIdEspecie = new SqlParameter("@IdEspecie", (object)idEspecie ?? DBNull.Value);
            SqlParameter pamIdRaza = new SqlParameter("@IdRaza", (object)idRaza ?? DBNull.Value);

            SqlParameter pamNuevoId = new SqlParameter();
            pamNuevoId.ParameterName = "@NuevoId";
            pamNuevoId.SqlDbType = SqlDbType.Int;
            pamNuevoId.Direction = ParameterDirection.Output;

            await this._context.Database.ExecuteSqlRawAsync(sql, pamIdUsuario, pamNombre, pamSexo, pamFechaNacimiento, pamPesoActual, pamImagen, pamIdEspecie, pamIdRaza, pamNuevoId);

            int nuevoId = (int)pamNuevoId.Value;

            return nuevoId;
        }
    }
}
