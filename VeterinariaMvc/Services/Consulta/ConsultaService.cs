using VeterinariaMvc.Dtos.Bloque;
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
    }
}