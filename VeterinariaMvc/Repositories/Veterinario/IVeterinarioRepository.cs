using VeterinariaMvc.Enums;
using VeterinariaMvc.Models;
using VeterinariaMvc.Models.Enums;

namespace VeterinariaMvc.Repositories.Clinica
{
    public interface IVeterinarioRepository
    {
        Task<int?> ObtenerIdClinicaDeUsuarioAsync(int idUsuario);
        Task<bool> RegistrarVeterinarioAsync(
            int idUsuario,
            int idClinica,
            string? numeroColegiado);

  
        Task<bool> EliminarVeterinarioAsync(int idUsuario, int idClinica);
    }
}
