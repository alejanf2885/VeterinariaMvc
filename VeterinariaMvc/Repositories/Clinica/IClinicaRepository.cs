using VeterinariaMvc.Dtos.Clinica;
using ModelClinica = VeterinariaMvc.Models.Clinica; 
namespace VeterinariaMvc.Repositories.Clinica
{
    public interface IClinicaRepository
    {
        Task<List<ModelClinica>> GetClinicasAsync();
        Task<ModelClinica?> GetClinicaPorIdAsync(int idClinica);

        Task<int> InsertarClinicaAsync(ModelClinica clinica);
        Task<bool> ConfigurarAgendaAsync(int idClinica, TimeSpan apertura, TimeSpan cierre, int duracionCita);
        Task<bool> EsClinicaConfiguradaAsync(int idUsuario);

        Task<int?> ObtenerIdPorUsuarioAdminAsync(int idUsuario);
    }
}
