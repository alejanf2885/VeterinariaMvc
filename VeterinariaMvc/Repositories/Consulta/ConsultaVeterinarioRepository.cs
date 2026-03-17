using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Data;
using VeterinariaMvc.Dtos.Consultas;

namespace VeterinariaMvc.Repositories.Consulta
{
    public class ConsultaVeterinarioRepository : IConsultaVeterinarioRepository
    {
        private readonly Context _context;

        public ConsultaVeterinarioRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<ConsultaVeterinarioDto>> GetCitasPorVeterinarioAsync(int idUsuarioVeterinario)
        {
            var query = from c in _context.CitasVeterinarioDashboard
                        where c.IdVeterinario == idUsuarioVeterinario
                        orderby c.FechaHoraConsulta
                        let idPlantillaFicha = _context.FichasConsulta.Where(f => f.IdConsulta == c.IdConsulta).Select(f => (int?)f.IdPlantilla).FirstOrDefault()
                        select new ConsultaVeterinarioDto
                        {
                            IdConsulta = c.IdConsulta,
                            IdClinica = c.IdClinica,
                            NombreClinica = c.NombreClinica ?? string.Empty,
                            IdMascota = c.IdMascota,
                            NombreMascota = c.NombreMascota ?? string.Empty,
                            ImagenMascota = c.ImagenMascota ?? string.Empty,
                            IdDueno = c.IdDueno,
                            NombreDueno = c.NombreDueno ?? string.Empty,
                            TelefonoDueno = c.TelefonoDueno ?? string.Empty,
                            EmailDueno = c.EmailDueno ?? string.Empty,
                            IdVeterinario = c.IdVeterinario,
                            FechaHoraConsulta = c.FechaHoraConsulta,
                            Turno = c.Turno ?? string.Empty,
                            Motivo = c.Motivo ?? string.Empty,
                            Estado = c.Estado ?? string.Empty,
                            IdPlantillaFicha = idPlantillaFicha
                        };

            return await query.ToListAsync();
        }
    }
}
