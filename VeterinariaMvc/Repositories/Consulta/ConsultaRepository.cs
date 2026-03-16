using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using VeterinariaMvc.Data;
using VeterinariaMvc.Dtos.Bloque;
using VeterinariaMvc.Dtos.Consultas.VeterinariaMvc.Dtos.Consulta;

namespace VeterinariaMvc.Repositories.Consulta
{
    public class ConsultaRepository : IConsultaRepository
    {
        private readonly Context _context;
        public ConsultaRepository(Context context) { _context = context; }

        public async Task<bool> CancelarConsultaAsync(int idConsulta)
        {
            ConsultaResumen consultaAEliminar = await this._context.ConsultasResumidas
                .FirstOrDefaultAsync(datos => datos.IdConsulta == idConsulta);

            if (consultaAEliminar == null)
            {
                return false;
            }

            consultaAEliminar.Estado = "CANCELADA";

            int filasAfectadas = await this._context.SaveChangesAsync();

            return filasAfectadas > 0;
        }

        public async Task<List<BloqueDisponibleDto>> GetBloquesDisponiblesAsync(int idClinica, DateTime fecha)
        {
            var consulta = from datos in this._context.BloquesDisponibles
                           where datos.IdClinica == idClinica && datos.Fecha == fecha
                           orderby datos.Turno
                           select datos;

            return await consulta.ToListAsync();
        }

        public async Task<ConsultaResumen> GetConsultaDetalleAsync(int idConsulta)
        {
            var consulta = from datos in this._context.ConsultasResumidas
                           where datos.IdConsulta == idConsulta
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task<List<ConsultaResumen>> GetConsultasByUserAsync(int idUsuario)
        {
            var consulta = from datos in this._context.ConsultasResumidas
                           where datos.IdUsuario == idUsuario
                           orderby datos.Fecha descending
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

        public async Task<List<DashboardCitaSinVeterinario>> GetCitasSinVeterinarioAsync(int idClinica)
        {
            var consulta = from datos in _context.CitasSinVeterinarioDashboard
                           where datos.IdClinica == idClinica
                           orderby datos.FechaHoraConsulta, datos.Turno
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<List<DashboardCitaVeterinario>> GetCitasPorVeterinarioAsync(int idClinica)
        {
            var consulta = from datos in _context.CitasVeterinarioDashboard
                           where datos.IdClinica == idClinica
                           orderby datos.FechaHoraConsulta, datos.Turno, datos.NombreVeterinario
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<bool> AsignarVeterinarioAsync(int idConsulta, int idVeterinario)
        {
            // Actualizamos directamente la tabla CONSULTA para guardar el ID del VETERINARIO
            string sql = "UPDATE dbo.CONSULTA SET ID_VETERINARIO = @IdVeterinario WHERE ID = @IdConsulta";

            var pamIdConsulta = new SqlParameter("@IdConsulta", idConsulta);
            var pamIdVeterinario = new SqlParameter("@IdVeterinario", idVeterinario);

            int filas = await _context.Database.ExecuteSqlRawAsync(sql, pamIdConsulta, pamIdVeterinario);
            return filas > 0;
        }

        public async Task<bool> ActualizarEstadoConsultaAsync(int idConsulta, string estado)
        {
            string sql = "UPDATE dbo.CONSULTA SET ESTADO = @Estado WHERE ID = @IdConsulta";
            var pamEstado = new SqlParameter("@Estado", estado);
            var pamIdConsulta = new SqlParameter("@IdConsulta", idConsulta);

            int filas = await _context.Database.ExecuteSqlRawAsync(sql, pamEstado, pamIdConsulta);
            return filas > 0;
        }
    }
}