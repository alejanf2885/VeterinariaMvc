using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Data;
using VeterinariaMvc.Dtos.Tratamiento;
using VeterinariaMvc.Models;

namespace VeterinariaMvc.Repositories.Tratamientos
{
    public class TratamientoRepository : ITratamientoRepository
    {
        private readonly Context _context;

        public TratamientoRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<TratamientoDto>> GetTratamientosPorMascotaAsync(int idMascota)
        {
            string query = "EXEC SP_GET_TRATAMIENTOS_POR_MASCOTA @IdMascota";
            SqlParameter parametro = new SqlParameter("@IdMascota", idMascota);

            return await _context.Set<TratamientoDto>()
                .FromSqlRaw(query, parametro)
                .ToListAsync();
        }

        public async Task<List<TratamientoDto>> GetTratamientosPorUsuarioAsync(int idUsuario)
        {
            string query = "EXEC SP_GET_TRATAMIENTOS_POR_USUARIO @IdUsuario";
            SqlParameter parametro = new SqlParameter("@IdUsuario", idUsuario);

            return await _context.Set<TratamientoDto>()
                .FromSqlRaw(query, parametro)
                .ToListAsync();
        }

        public async Task<TratamientoDto?> GetTratamientoDetalleAsync(int idTratamiento, int idUsuario)
        {
            string query = "EXEC SP_GET_TRATAMIENTO_DETALLE @IdTratamiento, @IdUsuario";
            SqlParameter paramIdTratamiento = new SqlParameter("@IdTratamiento", idTratamiento);
            SqlParameter paramIdUsuario = new SqlParameter("@IdUsuario", idUsuario);

            List<TratamientoDto> tratamientosDto = await _context.Set<TratamientoDto>()
                .FromSqlRaw(query, paramIdTratamiento, paramIdUsuario)
                .ToListAsync();

            TratamientoDto? tratamientoDto = tratamientosDto.FirstOrDefault();

            if (tratamientoDto != null)
            {
                tratamientoDto.Seguimientos = await GetSeguimientosPorTratamientoAsync(idTratamiento);
            }

            return tratamientoDto;
        }

        public async Task<List<SeguimientoDto>> GetSeguimientosPorTratamientoAsync(int idTratamiento)
        {
            string query = "EXEC SP_GET_SEGUIMIENTOS_POR_TRATAMIENTO @IdTratamiento";
            SqlParameter parametro = new SqlParameter("@IdTratamiento", idTratamiento);

            return await _context.Set<SeguimientoDto>()
                .FromSqlRaw(query, parametro)
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
    }
}