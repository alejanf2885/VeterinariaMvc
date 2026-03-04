using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Data;
using VeterinariaMvc.Dtos.Clinica;
using ModelClinica = VeterinariaMvc.Models.Clinica;


namespace VeterinariaMvc.Repositories.Clinica
{
    public class ClinicaRepository : IClinicaRepository
    {
        private Context _context;

        public ClinicaRepository(Context context)
        {
            _context = context;
        }

        public Task<ModelClinica?> GetClinicaPorIdAsync(int idClinica)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ModelClinica>> GetClinicasAsync()
        {
            var consulta = from datos in this._context.Clinicas
                           select datos;

            List<ModelClinica> clinicas = await consulta.ToListAsync();
            return clinicas;

        }
    }
}
