using VeterinariaMvc.Dtos.Bloque;
using VeterinariaMvc.Dtos.Consultas.VeterinariaMvc.Dtos.Consulta;
using VeterinariaMvc.Repositories.Consulta;

namespace VeterinariaMvc.Services.Consulta
{
    public class ConsultaService : IConsultaService
    {
        private  IConsultaRepository _repo;
        public ConsultaService(IConsultaRepository repo)
        {
            this._repo = repo;

        }

        public async Task<List<BloqueDisponibleDto>> ObtenerHorariosDisponiblesAsync(int idClinica, DateTime fecha)
        {
            if (fecha < DateTime.Today) return new List<BloqueDisponibleDto>();
            List<BloqueDisponibleDto> bloques = await _repo.GetBloquesDisponiblesAsync(idClinica, fecha);
            return bloques;
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

        public async Task<ConsultaResumen> GetConsultaDetalleAsync(int idConsulta, int idUsuario)
        {
            ConsultaResumen detalle =await this._repo.GetConsultaDetalleAsync(idConsulta);

            if (detalle == null || detalle.IdUsuario != idUsuario)
                return null;

            return detalle;




        }

        public async Task<bool> CancelarConsultaAsync(int idConsulta, int idUsuario)
        {
            ConsultaResumen consulta = await this._repo.GetConsultaDetalleAsync(idConsulta);

            if (consulta == null || consulta.IdUsuario != idUsuario || consulta.Estado.ToUpper() == "CANCELADA")
                return false;

            return await this._repo.CancelarConsultaAsync(idConsulta);
        }
    }
}