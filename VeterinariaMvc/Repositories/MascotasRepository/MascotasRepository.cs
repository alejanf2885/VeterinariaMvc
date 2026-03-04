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

        public async Task<Mascota?> GetMascotaEntityPorIdAsync(int idMascota)
        {
            return await this._context.Mascotas.FirstOrDefaultAsync(m => m.Id == idMascota);
        }

        public async Task<bool> ActualizarMascotaAsync(Mascota mascota)
        {
            string sql = "EXEC SP_ACTUALIZARMASCOTA @IdMascota, @Nombre, @Sexo, @FechaNacimiento, @PesoActual, @IdEspecie, @IdRaza, @Imagen, @Resultado OUTPUT";

            SqlParameter pamIdMascota = new SqlParameter("@IdMascota", mascota.Id);
            SqlParameter pamNombre = new SqlParameter("@Nombre", mascota.Nombre);
            SqlParameter pamSexo = new SqlParameter("@Sexo", (object)mascota.Sexo ?? DBNull.Value);
            SqlParameter pamFechaNac = new SqlParameter("@FechaNacimiento", (object)mascota.FechaNacimiento ?? DBNull.Value);
            SqlParameter pamPeso = new SqlParameter("@PesoActual", (object)mascota.PesoActual ?? DBNull.Value);
            SqlParameter pamEspecie = new SqlParameter("@IdEspecie", (object)mascota.IdEspecie ?? DBNull.Value);
            SqlParameter pamRaza = new SqlParameter("@IdRaza", (object)mascota.IdRaza ?? DBNull.Value);
            SqlParameter pamImagen = new SqlParameter("@Imagen", (object)mascota.Imagen ?? DBNull.Value);

            SqlParameter pamResultado = new SqlParameter();
            pamResultado.ParameterName = "@Resultado";
            pamResultado.SqlDbType = SqlDbType.Int;
            pamResultado.Direction = ParameterDirection.Output;


            await this._context.Database.ExecuteSqlRawAsync(sql,
                pamIdMascota, pamNombre, pamSexo, pamFechaNac, pamPeso, pamEspecie, pamRaza, pamImagen, pamResultado
            );

            return (int)pamResultado.Value == 1;
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
            string sql = "EXEC SP_INSERTARMASCOTA @IdUsuario, @Nombre, @Sexo, @FechaNacimiento, @PesoActual, @Imagen, @IdEspecie, @IdRaza, @NuevoId OUTPUT";

            SqlParameter pamIdUsuario = new SqlParameter("@IdUsuario", idUsuario);
            SqlParameter pamNombre = new SqlParameter("@Nombre", nombre);
            SqlParameter pamSexo = new SqlParameter("@Sexo", (object)Sexo ?? DBNull.Value);
            SqlParameter pamFechaNacimiento = new SqlParameter("@FechaNacimiento", (object)fechaNacimiento ?? DBNull.Value);
            SqlParameter pamPesoActual = new SqlParameter("@PesoActual", (object)pesoActual ?? DBNull.Value);
            SqlParameter pamImagen = new SqlParameter("@Imagen", imagen);
            SqlParameter pamIdEspecie = new SqlParameter("@IdEspecie", (object)idEspecie ?? DBNull.Value);
            SqlParameter pamIdRaza = new SqlParameter("@IdRaza", (object)idRaza ?? DBNull.Value);

            SqlParameter pamNuevoId = new SqlParameter
            {
                ParameterName = "@NuevoId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            await this._context.Database.ExecuteSqlRawAsync(sql,
                pamIdUsuario, pamNombre, pamSexo, pamFechaNacimiento,
                pamPesoActual, pamImagen, pamIdEspecie, pamIdRaza, pamNuevoId);

            int nuevoId = (int)pamNuevoId.Value;

            return nuevoId;
        }

    
    }
}
