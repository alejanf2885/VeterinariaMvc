using VeterinariaMvc.Dtos.Clinica;
using ModelClinica = VeterinariaMvc.Models.Clinica; 
namespace VeterinariaMvc.Repositories.Clinica
{
    public interface IClinicaRepository
    {
        Task<List<ModelClinica>> GetClinicasAsync();
        Task<ModelClinica?> GetClinicaPorIdAsync(int idClinica);
    }
}
