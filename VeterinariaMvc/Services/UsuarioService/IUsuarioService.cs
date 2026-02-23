using VeterinariaMvc.Models;

namespace VeterinariaMvc.Services.UsuarioService
{
    public interface IUsuarioService
    {
        Task<Usuario?> ValidarYObtenerUsuarioAsync(string email, string password);
    }
}
