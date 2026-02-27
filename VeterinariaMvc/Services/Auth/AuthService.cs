using VeterinariaMvc.Dtos.Auth;
using VeterinariaMvc.Enums;
using VeterinariaMvc.Models;
using VeterinariaMvc.Models.Auth;
using VeterinariaMvc.Repositories.Auth;
using VeterinariaMvc.Repositories.UsuarioRepository;
using VeterinariaMvc.Services.Criptografia;
using VeterinariaMvc.Services.Imagenes;
using VeterinariaMvc.Services.UsuarioService;

namespace VeterinariaMvc.Services.Auth
{
    public class AuthService : IAuthService
    {
        private IAuthUsuarioRepository authUsuarioRepository;
        private IPasswordHasher passwordHasher;
        private IUsuarioService usuarioService;
        private IImagenService imagenService;
        private IUsuarioRepository usuarioRepository; 



        public AuthService
            (IAuthUsuarioRepository authUsuarioRepository,
            IPasswordHasher passwordHasher,
            IUsuarioService usuarioService,
            IImagenService imagenService,
            IUsuarioRepository usuarioRepository)
        {
            this.authUsuarioRepository = authUsuarioRepository;
            this.passwordHasher = passwordHasher;
            this.usuarioService = usuarioService;
            this.imagenService = imagenService;
            this.usuarioRepository = usuarioRepository;
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
            (string email, string password, string nombre, string? telefono, IFormFile imagen)
        {

            //Existe ya el email a registrarse
            bool existe = await this.usuarioService.ExisteEmailAsync(email);

            if (existe)
            {
                throw new InvalidOperationException("El email ya esta registrado.");
            }

            string rutaFoto = "/images/usuarios/default-avatar.png";
            if(imagen != null && imagen.Length > 0)
            {
                 rutaFoto = await this.imagenService.SubirImagenAsync(imagen, CarpetaDestino.Usuarios);

            }

            string passwordHash = this.passwordHasher.HashearPassword(password);

            Usuario usuario = await this.usuarioRepository.RegistrarUsuarioAsync
                (email, nombre, telefono, rutaFoto, TipoCredencial.PASSWORD, passwordHash);


            return usuario;
        }
    }
}
