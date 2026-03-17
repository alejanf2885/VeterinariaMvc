using VeterinariaMvc.Dtos.Veterinarios;

namespace VeterinariaMvc.Services.Veterinarios
{
    public interface IVeterinarioService
    {
        Task<List<VeterinarioDto>> ObtenerVeterinariosPorClinicaAsync(int idClinica);

        Task<int?> ObtenerIdVeterinarioAsync(int idUsuario, int idClinica);

        Task<bool> RegistrarVeterinarioAsync(
         int idUsuario,
         int idClinica,
         string? numeroColegiado);


        Task<bool> EliminarVeterinarioAsync(int idUsuario, int idClinica);
    }
}
