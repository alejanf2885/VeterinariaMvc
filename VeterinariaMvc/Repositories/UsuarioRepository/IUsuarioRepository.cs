using VeterinariaMvc.Enums;
using VeterinariaMvc.Models;

namespace VeterinariaMvc.Repositories.UsuarioRepository
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObtenerPorEmailAsync(string email);
        Task<bool> ExisteEmailAsync(string email);

        Task<Usuario?> RegistrarUsuarioAsync
            (string email, string nombre, string telefono,
            string rutaImagen, TipoCredencial tipoAuth, string passwordHash);

        Task<Usuario?> ObtenerPorIdAsync(int id);
    }
}
