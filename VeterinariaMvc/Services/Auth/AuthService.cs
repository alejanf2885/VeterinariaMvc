using System;
using Microsoft.AspNetCore.Http;
using VeterinariaMvc.Dtos.Auth;
using VeterinariaMvc.Enums;
using VeterinariaMvc.Models;
using VeterinariaMvc.Models.Auth;
using VeterinariaMvc.Models.Enums;
using VeterinariaMvc.Repositories.Auth;
using VeterinariaMvc.Repositories.UsuarioRepository;
using VeterinariaMvc.Services.Criptografia;
using VeterinariaMvc.Services.Imagenes;
using VeterinariaMvc.Services.UsuarioService;

namespace VeterinariaMvc.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthUsuarioRepository _authRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUsuarioService _usuarioService;
        private readonly IImagenService _imagenService;
        private readonly IUsuarioRepository _usuarioRepo;

        public AuthService(
            IAuthUsuarioRepository authRepo,
            IPasswordHasher passwordHasher,
            IUsuarioService usuarioService,
            IImagenService imagenService,
            IUsuarioRepository usuarioRepo)
        {
            _authRepo = authRepo;
            _passwordHasher = passwordHasher;
            _usuarioService = usuarioService;
            _imagenService = imagenService;
            _usuarioRepo = usuarioRepo;
        }

        public async Task<Usuario?> LoginAsync(string email, string password)
        {
            AuthUsuario? auth = await _authRepo.ObtenerPorEmailAsync(email);
            if (auth == null || !auth.Activo) return null;

            if (!_passwordHasher.VerificarPassword(password, auth.PasswordHash))
                return null;

            return await _usuarioService.ObtenerPorEmailAsync(email);
        }

        public Task<Usuario?> RegisterUsuarioAsync(RegisterDto dto)
        {
            // Ahora le pasamos el dto.Rol a RegistrarCoreAsync
            return RegistrarCoreAsync(dto.Email, dto.Password, dto.Nombre, dto.Telefono, dto.Imagen, dto.Rol);
        }

        private async Task<Usuario?> RegistrarCoreAsync(
            string email, string password, string nombre, string? telefono, IFormFile? imagen, Roles rol) 
        {
            // 1) Validar que no exista el email
            if (await _usuarioService.ExisteEmailAsync(email))
                throw new InvalidOperationException("El email ya está registrado.");

            // 2) Procesar imagen
            string rutaFoto = "/images/usuarios/default-avatar.png";
            if (imagen != null && imagen.Length > 0)
            {
                rutaFoto = await _imagenService.SubirImagenAsync(imagen, CarpetaDestino.Usuarios, 500);
            }

            // 3) Hashear contraseña
            string passwordHash = _passwordHasher.HashearPassword(password);

            // 4) Crear usuario
            Usuario? usuario = await _usuarioRepo.RegistrarUsuarioAsync(
                email,
                nombre,
                telefono ?? string.Empty,
                rutaFoto,
                TipoCredencial.PASSWORD,
                rol, 
                passwordHash);

            return usuario;
        }
    }
}