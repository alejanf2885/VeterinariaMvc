using MvcCoreSession.Extensions;
using VeterinariaMvc.Dtos.Session;
using VeterinariaMvc.Models;
using VeterinariaMvc.Services.Clinica;

namespace VeterinariaMvc.Services.Estado
{
    public class SessionUsuarioService : IEstadoUsuarioService
    {

        private IHttpContextAccessor contextAccessor;
        private IClinicaService clinicaService;

        public SessionUsuarioService(IHttpContextAccessor contextAccessor, IClinicaService clinicaService)
        {
            this.contextAccessor = contextAccessor;
            this.clinicaService = clinicaService;
        }

        public async Task DestruirSesionAsync()
        {
            this.contextAccessor.HttpContext.Session.Remove("USUARIOACTUAL");
        }

        public Task<bool> EsClinicaConfiguradaAsync(int idUsuario)
        {
            return clinicaService.EsClinicaConfiguradaAsync(idUsuario);
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
