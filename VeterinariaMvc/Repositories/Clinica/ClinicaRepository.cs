using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Data;
using ModelClinica = VeterinariaMvc.Models.Clinica;

namespace VeterinariaMvc.Repositories.Clinica
{
    public class ClinicaRepository : IClinicaRepository
    {
        private readonly Context _context;

        public ClinicaRepository(Context context)
        {
            _context = context;
        }

        public async Task<ModelClinica?> GetClinicaPorIdAsync(int idClinica)
        {
            return await _context.Clinicas
                .FirstOrDefaultAsync(c => c.Id == idClinica);
        }

        public async Task<int> InsertarClinicaAsync(ModelClinica clinica)
        {
            clinica.Configurado = true;
            _context.Clinicas.Add(clinica);
            await _context.SaveChangesAsync();
            return clinica.Id;
        }

        public async Task<bool> ConfigurarAgendaAsync(int idClinica, TimeSpan apertura, TimeSpan cierre, int duracionCita)
        {
            string sql = "EXEC SP_ConfigurarAgendaParaSiempre @IdClinica, @HoraApertura, @HoraCierre, @MinutosPorCita";

            var parameters = new[]
            {
                new SqlParameter("@IdClinica", idClinica),
                new SqlParameter("@HoraApertura", apertura),
                new SqlParameter("@HoraCierre", cierre),
                new SqlParameter("@MinutosPorCita", duracionCita)
            };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(sql, parameters);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<ModelClinica>> GetClinicasAsync()
        {
            return await _context.Clinicas.ToListAsync();
        }

        public async Task<bool> EsClinicaConfiguradaAsync(int idUsuario)
        {
            return await _context.Clinicas.AnyAsync(c => c.IdUsuario == idUsuario);

        }

        public async Task<int?> ObtenerIdPorUsuarioAdminAsync(int idUsuario)
        {
            var clinica = await this._context.Clinicas
                .FirstOrDefaultAsync(c => c.IdUsuario == idUsuario);

            return clinica?.Id;
        }
    }
}