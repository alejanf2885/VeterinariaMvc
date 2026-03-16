using VeterinariaMvc.Dtos.Clinica;
using ModelClinica = VeterinariaMvc.Models.Clinica;


namespace VeterinariaMvc.Services.Clinica
{
    public interface IClinicaService
    {
        Task<int?> ObtenerIdClinicaDeUsuarioAsync(int idUsuario);

        Task<List<ClinicaDto>> GetClinicasAsync();
        Task<int> RegistrarNuevaClinicaAsync(ModelClinica clinica, string emailAdmin,  TimeSpan apertura, TimeSpan cierre, int duracion);
        Task<bool> EsClinicaConfiguradaAsync(int idUsuario);

    }
}
