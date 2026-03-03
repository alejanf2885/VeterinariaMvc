using VeterinariaMvc.Models;

namespace VeterinariaMvc.Repositories.EspecieRepository
{
    public interface IEspecieRepository 
    {
        Task<List<Especie>> GetEspeciesAsync();
    }
}
