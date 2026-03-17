using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using VeterinariaMvc.Dtos.Session;
using VeterinariaMvc.Services.Clinica;

namespace VeterinariaMvc.Services.Estado
{
    public class ClaimsUsuarioService : IEstadoUsuarioService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IClinicaService _clinicaService;

        public ClaimsUsuarioService(IHttpContextAccessor contextAccessor, IClinicaService clinicaService)
        {
            _contextAccessor = contextAccessor;
            _clinicaService = clinicaService;
        }

        public async Task DestruirSesionAsync()
        {
            if (_contextAccessor.HttpContext != null)
            {
                await _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }

        public Task<bool> EsClinicaConfiguradaAsync(int idUsuario)
        {
            return _clinicaService.EsClinicaConfiguradaAsync(idUsuario);
        }

        public async Task GuardarSesionAsync(UsuarioSessionDto usuario)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre ?? ""),
                new Claim(ClaimTypes.Email, usuario.Email ?? ""),
                new Claim(ClaimTypes.Role, usuario.IdRol.ToString()),
                new Claim("Imagen", usuario.Imagen ?? "")
            };

            // ¡AÑADIMOS EL ID DE LA CLÍNICA SI EXISTE!
            if (usuario.IdClinica.HasValue)
            {
                claims.Add(new Claim("IdClinica", usuario.IdClinica.Value.ToString()));
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            if (_contextAccessor.HttpContext != null)
            {
                await _contextAccessor.HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
            }
        }

        public async Task<UsuarioSessionDto?> ObtenerUsuarioActualAsync()
        {
            var user = _contextAccessor.HttpContext?.User;

            if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            // Recuperamos el claim de la clínica
            var idClinicaClaim = user.FindFirst("IdClinica")?.Value;

            UsuarioSessionDto usuarioDto = new UsuarioSessionDto
            {
                Id = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0"),
                Nombre = user.FindFirst(ClaimTypes.Name)?.Value,
                Email = user.FindFirst(ClaimTypes.Email)?.Value,
                IdRol = int.Parse(user.FindFirst(ClaimTypes.Role)?.Value ?? "1"),
                Imagen = user.FindFirst("Imagen")?.Value,
                // Parseamos el IdClinica si existe en la cookie
                IdClinica = string.IsNullOrEmpty(idClinicaClaim) ? null : int.Parse(idClinicaClaim)
            };

            return usuarioDto;
        }
    }
}