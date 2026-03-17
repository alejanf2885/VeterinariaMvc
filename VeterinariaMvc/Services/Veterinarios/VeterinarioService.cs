using VeterinariaMvc.Repositories.Clinica;

namespace VeterinariaMvc.Services.Veterinarios
{
    public class VeterinarioService : IVeterinarioService
    {
        private readonly IVeterinarioRepository _veterinarioRepository;
        public VeterinarioService(IVeterinarioRepository veterinarioRepository)
        {
            _veterinarioRepository = veterinarioRepository;
        }

        public Task<System.Collections.Generic.List<VeterinariaMvc.Dtos.Veterinarios.VeterinarioDto>> ObtenerVeterinariosPorClinicaAsync(int idClinica)
        {
            return _veterinarioRepository.ObtenerVeterinariosPorClinicaAsync(idClinica);
        }

        public Task<int?> ObtenerIdVeterinarioAsync(int idUsuario, int idClinica)
        {
            return _veterinarioRepository.ObtenerIdVeterinarioAsync(idUsuario, idClinica);
        }

        public Task<bool> EliminarVeterinarioAsync(int idUsuario, int idClinica)
        {
            return _veterinarioRepository.EliminarVeterinarioAsync(idUsuario, idClinica);
        }

        public Task<bool> RegistrarVeterinarioAsync(int idUsuario, int idClinica, string? numeroColegiado)
        {
            return _veterinarioRepository.RegistrarVeterinarioAsync(idUsuario, idClinica, numeroColegiado);
        }
    }
}
