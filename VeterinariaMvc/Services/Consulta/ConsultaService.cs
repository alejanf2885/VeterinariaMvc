using VeterinariaMvc.Dtos.Bloque;
using VeterinariaMvc.Dtos.Consultas.VeterinariaMvc.Dtos.Consulta;
using VeterinariaMvc.Repositories.Consulta;

namespace VeterinariaMvc.Services.Consulta
{
    public class ConsultaService : IConsultaService
    {
        private readonly IConsultaRepository _repo;

        public ConsultaService(IConsultaRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<BloqueDisponibleDto>> ObtenerHorariosDisponiblesAsync(int idClinica, DateTime fecha)
        {
            if (fecha < DateTime.Today) return new List<BloqueDisponibleDto>();
            return await _repo.GetBloquesDisponiblesAsync(idClinica, fecha);
        }

        public async Task<bool> CrearReservaAsync(int idMascota, int idClinica, int idBloque, string motivo)
        {
            int resultado = await _repo.ReservarConsultaAsync(idMascota, idClinica, idBloque, motivo);
            return resultado > 0;
        }

        public async Task<List<ConsultaResumen>> GetConsultasDashboardAsync(int idUsuario)
        {
            List<ConsultaResumen> todas = await _repo.GetConsultasByUserAsync(idUsuario);

            return todas
                .Where(c => c.Estado.ToUpper() != "CANCELADA" && c.Fecha >= DateTime.Now)
                .OrderBy(c => c.Fecha) // La más próxima primero
                .ToList();
        }

        public async Task<List<ConsultaResumen>> GetHistorialCompletoAsync(int idUsuario)
        {
            return await _repo.GetConsultasByUserAsync(idUsuario);
        }

        public async Task<ConsultaResumen> GetConsultaDetalleAsync(int idConsulta)
        {
            return await _repo.GetConsultaDetalleAsync(idConsulta);
        }

        public async Task<bool> CancelarConsultaAsync(int idConsulta)
        {
            ConsultaResumen consulta = await _repo.GetConsultaDetalleAsync(idConsulta);

            if (consulta == null || consulta.Estado.ToUpper() == "CANCELADA")
                return false;

            return await _repo.CancelarConsultaAsync(idConsulta);
        }

        public async Task<List<DashboardCitaSinVeterinario>> GetCitasSinVeterinarioDashboardAsync(int idClinica)
        {
            return await _repo.GetCitasSinVeterinarioAsync(idClinica);
        }

        public async Task<List<DashboardCitaVeterinario>> GetCitasPorVeterinarioDashboardAsync(int idClinica)
        {
            return await _repo.GetCitasPorVeterinarioAsync(idClinica);
        }

        public async Task<bool> AsignarVeterinarioAsync(int idConsulta, int idVeterinario)
        {
            return await _repo.AsignarVeterinarioAsync(idConsulta, idVeterinario);
        }
    }
}