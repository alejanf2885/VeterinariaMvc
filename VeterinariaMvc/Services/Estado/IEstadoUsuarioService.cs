using VeterinariaMvc.Dtos.Session;
using VeterinariaMvc.Models;

namespace VeterinariaMvc.Services.Estado
{
    public interface IEstadoUsuarioService
    {
        Task GuardarSesionAsync(UsuarioSessionDto usuario);
        Task DestruirSesionAsync();
        Task<UsuarioSessionDto?> ObtenerUsuarioActualAsync();

        Task<bool> EsClinicaConfiguradaAsync(int idUsuario);

    }
}
