using VeterinariaMvc.Dtos.Clinica;
using ModelClinica = VeterinariaMvc.Models.Clinica;


namespace VeterinariaMvc.Services.Clinica
{
    public interface IClinicaService
    {
        Task<int?> ObtenerIdClinicaDeUsuarioAsync(int idUsuario);

        Task<List<ClinicaDto>> GetClinicasAsync();
    }
}
