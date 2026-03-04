
using VeterinariaMvc.Dtos.Clinica;
using VeterinariaMvc.Mappers;
using VeterinariaMvc.Models;
using VeterinariaMvc.Repositories.Clinica;
using ModelClinica = VeterinariaMvc.Models.Clinica;


namespace VeterinariaMvc.Services.Clinica
{
    public class ClinicaService : IClinicaService
    {
        private IVeterinarioRepository veterinarioRepository;
        private IClinicaRepository clinicaRepository;
        public ClinicaService(IVeterinarioRepository veterinarioRepository, IClinicaRepository clinicaRepository)
        {
            this.veterinarioRepository = veterinarioRepository;
            this.clinicaRepository = clinicaRepository;
        }

        public async Task<List<ClinicaDto>> GetClinicasAsync()
        {
            List<ModelClinica> clinicas = await this.clinicaRepository.GetClinicasAsync();

            List<ClinicaDto> clinicasDto = clinicas
            .Select(c => c.ToClinicaDto())
            .ToList();

            return clinicasDto;
        }

        public async Task<int?> ObtenerIdClinicaDeUsuarioAsync(int idUsuario)
        {
            int? idClinica = await this.veterinarioRepository.ObtenerIdClinicaDeUsuarioAsync(idUsuario);
            return idClinica;
        }

       
    }
}
