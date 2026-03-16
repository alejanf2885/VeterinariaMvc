using VeterinariaMvc.Dtos.Clinica;
using VeterinariaMvc.Dtos.Auth;
using VeterinariaMvc.Mappers;
using VeterinariaMvc.Models;
using VeterinariaMvc.Repositories.Clinica;
using ModelClinica = VeterinariaMvc.Models.Clinica;
using VeterinariaMvc.Models.Enums;
using VeterinariaMvc.Services.Auth;

namespace VeterinariaMvc.Services.Clinica
{
    public class ClinicaService : IClinicaService
    {
        private readonly IVeterinarioRepository _veterinarioRepository;
        private readonly IClinicaRepository _clinicaRepository;
        private readonly IAuthService _authService;

        public ClinicaService(
            IVeterinarioRepository veterinarioRepository,
            IClinicaRepository clinicaRepository,
            IAuthService authService)
        {
            _veterinarioRepository = veterinarioRepository;
            _clinicaRepository = clinicaRepository;
            _authService = authService;
        }

      
        public async Task<int> RegistrarNuevaClinicaAsync(ModelClinica clinica, string emailAdmin, string passwordAdmin, TimeSpan apertura, TimeSpan cierre, int duracion)
        {
            int idClinica = await _clinicaRepository.InsertarClinicaAsync(clinica);

            var registerDto = new RegisterDto
            {
                Email = emailAdmin,
                Password = passwordAdmin,
                Nombre = $"Admin {clinica.Nombre}",
                Rol = Roles.AdminClinica
            };

            await _authService.RegisterUsuarioAsync(registerDto);

            await _clinicaRepository.ConfigurarAgendaAsync(idClinica, apertura, cierre, duracion);

            return idClinica;
        }

        
        public async Task<List<ClinicaDto>> GetClinicasAsync()
        {
            var clinicas = await _clinicaRepository.GetClinicasAsync();

            return clinicas.Select(c => c.ToClinicaDto()).ToList();
        }

    
        public async Task<int?> ObtenerIdClinicaDeUsuarioAsync(int idUsuario)
        {
            return await _veterinarioRepository.ObtenerIdClinicaDeUsuarioAsync(idUsuario);
        }
    }
}