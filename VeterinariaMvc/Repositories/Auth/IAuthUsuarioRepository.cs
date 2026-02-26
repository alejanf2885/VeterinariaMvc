using VeterinariaMvc.Models;
using VeterinariaMvc.Models.Auth;

namespace VeterinariaMvc.Repositories.Auth
{
    public interface IAuthUsuarioRepository
    {

        Task<AuthUsuario?> ObtenerPorEmailAsync(string email);
        Task<bool> ExisteEmailAsync(string email);

    }
}
