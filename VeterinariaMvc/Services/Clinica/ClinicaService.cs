
using VeterinariaMvc.Repositories.Clinica;

namespace VeterinariaMvc.Services.Clinica
{
    public class ClinicaService : IClinicaService
    {
        private IVeterinarioRepository veterinarioRepository;
        public ClinicaService(IVeterinarioRepository veterinarioRepository)
        {
            this.veterinarioRepository = veterinarioRepository;
        }
        public async Task<int?> ObtenerIdClinicaDeUsuarioAsync(int idUsuario)
        {
            int? idClinica = await this.veterinarioRepository.ObtenerIdClinicaDeUsuarioAsync(idUsuario);
            return idClinica;
        }
    }
}
