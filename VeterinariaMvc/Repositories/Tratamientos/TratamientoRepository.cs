using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Data;
using VeterinariaMvc.Dtos.Tratamiento;
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
           List<TratamientoView> tratamientos = await _context.TratamientosView
                .Where(t => t.IdMascota == idMascota)
                .ToListAsync();
            return tratamientos;
        }

        public async Task<List<TratamientoView>> GetTratamientosPorUsuarioAsync(int idUsuario)
        {
            List<TratamientoView> tratamientos = await _context.TratamientosView
                .Where(t => t.IdUsuario == idUsuario)
                .ToListAsync();
            return tratamientos;
        }

        public async Task<TratamientoView?> GetTratamientoDetalleAsync(int idTratamiento, int idUsuario)
        {
            TratamientoView? tratamiento = await _context.TratamientosView
                .FirstOrDefaultAsync(t => t.Id == idTratamiento && t.IdUsuario == idUsuario);
            return tratamiento;
        }

        public async Task<List<SeguimientoView>> GetSeguimientosPorTratamientoAsync(int idTratamiento)
        {
           List<SeguimientoView> seguimientos = await _context.SeguimientosView
                .Where(s => s.IdTratamiento == idTratamiento)
                .OrderByDescending(s => s.Fecha)
                .ToListAsync();
            return seguimientos;
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
    }
}