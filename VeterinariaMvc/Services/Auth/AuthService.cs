using VeterinariaMvc.Models;
using VeterinariaMvc.Services.Criptografia;
using VeterinariaMvc.Services.UsuarioService;

namespace VeterinariaMvc.Services.Auth
{
    public class AuthService : IAuthService
    {

        private IUsuarioService usuarioService;
        private IPasswordHasher passwordHasher;

        public AuthService(IUsuarioService usuarioService, IPasswordHasher passwordHasher)
        {
            this.usuarioService = usuarioService;
            this.passwordHasher = passwordHasher;
        }

        public async Task<Usuario?> LoginAsync(string email, string password)
        {

            Usuario usuario = await this.usuarioService.ObtenerPorEmailAsync(email);

            if (usuario == null) return null;
            if (usuario.Activo == false) return null;

            bool valido = this.passwordHasher.VerificarPassword(password, usuario.Password);


            return valido ? usuario : null;

        }
    }
}
