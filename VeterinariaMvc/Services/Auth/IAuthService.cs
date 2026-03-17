using VeterinariaMvc.Dtos.Auth;
using VeterinariaMvc.Dtos.Veterinarios;
using VeterinariaMvc.Models;

namespace VeterinariaMvc.Services.Auth
{
    public interface IAuthService
    {
        Task<Usuario?> LoginAsync(string email, string password);
        Task<Usuario?> RegisterUsuarioAsync(RegisterDto dto);

        Task<Usuario?> RegistrarVeterinarioAsync(RegisterVeterinarioDto dto);
    }
}