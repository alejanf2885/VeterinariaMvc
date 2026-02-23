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

        public async Task<Usuario?> ValidarYObtenerUsuarioAsync(string email, string password)
        {

            Usuario usuario = await this.repo.ObtenerPorEmailAsync(email);

            if (usuario != null && usuario.Password == password)
            {
                return usuario;
            }
            return null;
        }
    }
}
