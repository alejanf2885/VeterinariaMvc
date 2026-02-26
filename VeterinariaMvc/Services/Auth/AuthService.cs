using VeterinariaMvc.Models;
using VeterinariaMvc.Models.Auth;
using VeterinariaMvc.Repositories.Auth;
using VeterinariaMvc.Services.Criptografia;
using VeterinariaMvc.Services.UsuarioService;

namespace VeterinariaMvc.Services.Auth
{
    public class AuthService : IAuthService
    {
        private IAuthUsuarioRepository authUsuarioRepository;
        private IPasswordHasher passwordHasher;
        private IUsuarioService usuarioService;

        public AuthService
            (IAuthUsuarioRepository authUsuarioRepository,
            IPasswordHasher passwordHasher,
            IUsuarioService usuarioService)
        {
            this.authUsuarioRepository = authUsuarioRepository;
            this.passwordHasher = passwordHasher;
            this.usuarioService = usuarioService;
        }

        public async Task<Usuario?> LoginAsync(string email, string password)
        {

            AuthUsuario usuario = await this.authUsuarioRepository.ObtenerPorEmailAsync(email);

            if (usuario == null) return null;
            if (usuario.Activo == false) return null;

            bool valido = this.passwordHasher.VerificarPassword(password, usuario.PasswordHash);

            if (valido)
            {
                Usuario usuarioFinal = await this.usuarioService.ObtenerPorEmailAsync(email);
                return usuarioFinal;
            }

            return null;

        }

        public async Task<Usuario?> RegisterAsync
            (string email, string password, string nombre, string telefono, string Imagen)
        {
            throw new NotImplementedException();
        }
    }
}
