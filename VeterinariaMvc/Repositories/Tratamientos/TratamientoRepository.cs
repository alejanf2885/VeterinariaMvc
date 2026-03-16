using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Data;
using VeterinariaMvc.Models;
using VeterinariaMvc.Models.Seguimientos;
using VeterinariaMvc.Models.Tratamientos;

namespace VeterinariaMvc.Repositories.Tratamientos
{
    public class TratamientoRepository : ITratamientoRepository
    {
        private readonly Context _context;

        public TratamientoRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<TratamientoView>> GetTratamientosPorMascotaAsync(int idMascota)
        {
            return await _context.TratamientosView
                .Where(t => t.IdMascota == idMascota)
                .ToListAsync();
        }

        public async Task<List<TratamientoView>> GetTratamientosPorUsuarioAsync(int idUsuario)
        {
            return await _context.TratamientosView
                .Where(t => t.IdUsuario == idUsuario)
                .ToListAsync();
        }

        public async Task<TratamientoView?> GetTratamientoDetalleAsync(int idTratamiento)
        {
            return await _context.TratamientosView
                .FirstOrDefaultAsync(t => t.Id == idTratamiento);
        }

        public async Task<List<SeguimientoView>> GetSeguimientosPorTratamientoAsync(int idTratamiento)
        {
            return await _context.SeguimientosView
                .Where(s => s.IdTratamiento == idTratamiento)
                .OrderByDescending(s => s.Fecha)
                .ToListAsync();
        }

        public async Task<bool> AgregarSeguimientoAsync(int idTratamiento, int idUsuario, string comentario)
        {
            SeguimientoTratamiento nuevoSeguimiento = new SeguimientoTratamiento
            {
                IdTratamiento = idTratamiento,
                IdUsuario = idUsuario,
                Comentario = comentario,
                Fecha = DateTime.Now
            };

            try
            {
                await _context.SeguimientosTratamiento.AddAsync(nuevoSeguimiento);
                int filasAfectadas = await _context.SaveChangesAsync();
                return filasAfectadas > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CrearTratamientoAsync(
            int idMascota,
            int idVeterinario,
            int idConsulta,
            string nombre,
            string? descripcion,
            DateTime fechaInicio,
            DateTime? fechaFin)
        {
            var tratamiento = new Tratamiento
            {
                IdMascota = idMascota,
                IdVeterinario = idVeterinario,
                IdConsulta = idConsulta,
                Nombre = nombre,
                Descripcion = descripcion,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Estado = "ACTIVO"
            };

            try
            {
                await _context.Tratamientos.AddAsync(tratamiento);
                int filasAfectadas = await _context.SaveChangesAsync();
                return filasAfectadas > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}