using VeterinariaMvc.Models;

namespace VeterinariaMvc.Services.UsuarioService
{
    public interface IUsuarioService
    {
        Task<Usuario?> ObtenerPorEmailAsync(string email);
    }
}
