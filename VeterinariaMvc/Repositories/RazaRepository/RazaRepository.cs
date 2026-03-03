using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Data;
using VeterinariaMvc.Models;

namespace VeterinariaMvc.Repositories.RazaRepository
{
    public class RazaRepository : IRazaRepository
    {
        private Context _context;
        public RazaRepository(Context context)
        {
            _context = context;
        }
        public async Task<List<Raza>> GetRazasAsync()
        {
            var consulta = from datos in this._context.Razas
                           select datos;

            return await consulta.ToListAsync();
        }
    }
}
