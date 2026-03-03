using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Data;
using VeterinariaMvc.Models;

namespace VeterinariaMvc.Repositories.EspecieRepository
{
    public class EspecieRepository : IEspecieRepository
    {
        private Context _context;
        public EspecieRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<Especie>> GetEspeciesAsync()
        {
            var consulta = from datos in this._context.Especies
                           select datos;

            return await consulta.ToListAsync();
        }
    }
}
