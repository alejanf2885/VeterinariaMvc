using System.Collections.Generic;
using System.Threading.Tasks;
using VeterinariaMvc.Dtos.Consultas;

namespace VeterinariaMvc.Repositories.Consulta
{
    public interface IConsultaVeterinarioRepository
    {
        Task<List<ConsultaVeterinarioDto>> GetCitasPorVeterinarioAsync(int idUsuarioVeterinario);
    }
}
