using VeterinariaMvc.Models;

namespace VeterinariaMvc.Repositories.RazaRepository
{
    public interface IRazaRepository
    {
        Task<List<Raza>> GetRazasAsync();
    }
}
