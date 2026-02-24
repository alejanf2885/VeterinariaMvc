using VeterinariaMvc.Models;
using VeterinariaMvc.Repositories.UsuarioRepository;

namespace VeterinariaMvc.Services.UsuarioService
{
    public class UsuarioService : IUsuarioService
    {

        private IUsuarioRepository repo;

        public UsuarioService(IUsuarioRepository repo)
        {
            this.repo = repo;
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            bool existe = await this.repo.ExisteEmailAsync(email);

            return existe;
        }

        public async Task<Usuario?> ObtenerPorEmailAsync(string email)
        {
            Usuario usuario = await this.repo.ObtenerPorEmailAsync(email);

            return usuario;
        }
    }
}
