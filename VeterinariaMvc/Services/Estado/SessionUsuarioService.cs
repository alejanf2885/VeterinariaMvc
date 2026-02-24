using MvcCoreSession.Extensions;
using VeterinariaMvc.Dtos.Session;
using VeterinariaMvc.Models;

namespace VeterinariaMvc.Services.Estado
{
    public class SessionUsuarioService : IEstadoUsuarioService
    {

        private IHttpContextAccessor contextAccessor;

        public SessionUsuarioService(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public async Task DestruirSesionAsync()
        {
            this.contextAccessor.HttpContext.Session.Remove("USUARIOACTUAL");
        }

        public async Task GuardarSesionAsync(UsuarioSessionDto usuario)
        {
           this.contextAccessor.HttpContext.Session.SetObject("USUARIOACTUAL", usuario);
        }

        public async Task<UsuarioSessionDto?> ObtenerUsuarioActualAsync()
        {
            UsuarioSessionDto usuarioSessionDto =
                this.contextAccessor.HttpContext.Session.GetObject<UsuarioSessionDto>("USUARIOACTUAL");
            return usuarioSessionDto;
        }
    }
}
