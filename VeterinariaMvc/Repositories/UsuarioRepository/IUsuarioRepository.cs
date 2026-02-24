using VeterinariaMvc.Models;

namespace VeterinariaMvc.Repositories.UsuarioRepository
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObtenerPorEmailAsync(string email);
    }
}
