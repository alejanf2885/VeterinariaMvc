using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using VeterinariaMvc.Data;
using VeterinariaMvc.Dtos.Bloque;

namespace VeterinariaMvc.Repositories.Consulta
{
    public class ConsultaRepository : IConsultaRepository
    {
        private readonly Context _context;
        public ConsultaRepository(Context context) { _context = context; }

        public async Task<List<BloqueDisponibleDto>> GetBloquesDisponiblesAsync(int idClinica, DateTime fecha)
        {
            var consulta = from datos in this._context.BloquesDisponibles
                           where datos.IdClinica == idClinica && datos.Fecha == fecha
                           orderby datos.Turno
                           select datos;

            return await consulta.ToListAsync();
        }

        public async Task<int> ReservarConsultaAsync(int idMascota, int idClinica, int idBloque, string motivo)
        {
            string sql = "EXEC SP_ReservarConsulta @IdMascota, @IdClinica, @IdBloque, @Motivo, @NuevoId OUTPUT";
            var pamIdOut = new SqlParameter("@NuevoId", SqlDbType.Int) { Direction = ParameterDirection.Output };

            SqlParameter pamIdMascota = new SqlParameter("@IdMascota", idMascota);
            SqlParameter pamIdClinica = new SqlParameter("@IdClinica", idClinica);
            SqlParameter pamIdBloque = new SqlParameter("@IdBloque", idBloque);
            SqlParameter pamMotivo = new SqlParameter("@Motivo", motivo);



            await this._context.Database.ExecuteSqlRawAsync(sql, pamIdMascota, pamIdClinica, pamIdBloque, pamMotivo, pamIdOut);

            return (int)pamIdOut.Value;
        }
    }
}