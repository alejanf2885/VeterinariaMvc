using VeterinariaMvc.Models;

namespace VeterinariaMvc.Services.Auth
{
    public interface IAuthService
    {

        Task<Usuario?> LoginAsync(string email, string password);
        Task<Usuario?> RegisterAsync
            (string email, string password,
            string nombre, string telefono,
            string Imagen);
    }
}
