using VeterinariaMvc.Dtos.Tratamiento;
using VeterinariaMvc.Models.Seguimientos;
using VeterinariaMvc.Models.Tratamientos;
using VeterinariaMvc.Repositories.Tratamientos;

namespace VeterinariaMvc.Services.Tratamientos
{
    public class TratamientoService : ITratamientoService
    {
        private readonly ITratamientoRepository _tratamientoRepository;

        public TratamientoService(ITratamientoRepository tratamientoRepository)
        {
            _tratamientoRepository = tratamientoRepository;
        }

        public async Task<List<TratamientoDto>> GetTratamientosPorMascotaAsync(int idMascota, int idUsuario)
        {
            List<TratamientoView> tratamientos = await _tratamientoRepository.GetTratamientosPorMascotaAsync(idMascota);
            return await MapTratamientosAListaDto(tratamientos);
        }

        public async Task<List<TratamientoDto>> GetTratamientosPorUsuarioAsync(int idUsuario)
        {
            List<TratamientoView> tratamientos = await _tratamientoRepository.GetTratamientosPorUsuarioAsync(idUsuario);
            return await MapTratamientosAListaDto(tratamientos);
        }

        public async Task<TratamientoDto?> GetTratamientoDetalleAsync(int idTratamiento, int idUsuario)
        {
            TratamientoView tratamiento = await _tratamientoRepository.GetTratamientoDetalleAsync(idTratamiento, idUsuario);
            if (tratamiento == null) return null;

            List<SeguimientoDto> seguimientos = await GetSeguimientosPorTratamientoAsync(tratamiento.Id);

            return MapTratamientoViewToDto(tratamiento, seguimientos);
        }

        public async Task<bool> AgregarSeguimientoAsync(int idTratamiento, int idUsuario, string comentario)
        {
            if (string.IsNullOrWhiteSpace(comentario))
                return false;

            return await _tratamientoRepository.AgregarSeguimientoAsync(idTratamiento, idUsuario, comentario);
        }

        public async Task<List<SeguimientoDto>> GetSeguimientosPorTratamientoAsync(int idTratamiento)
        {
            List<SeguimientoView> seguimientosView = await _tratamientoRepository.GetSeguimientosPorTratamientoAsync(idTratamiento);
            return seguimientosView.ConvertAll(MapSeguimientoViewToDto);
        }

        private async Task<List<TratamientoDto>> MapTratamientosAListaDto(List<TratamientoView> tratamientos)
        {
            List<TratamientoDto> listaDto = new List<TratamientoDto>();

            foreach (TratamientoView t in tratamientos)
            {
                List<SeguimientoDto> seguimientos = await GetSeguimientosPorTratamientoAsync(t.Id);
                listaDto.Add(MapTratamientoViewToDto(t, seguimientos));
            }

            return listaDto;
        }

        private static TratamientoDto MapTratamientoViewToDto(TratamientoView t, List<SeguimientoDto> seguimientos)
        {
            return new TratamientoDto
            {
                Id = t.Id,
                IdMascota = t.IdMascota,
                NombreMascota = t.NombreMascota,
                IdVeterinario = t.IdVeterinario,
                IdUsuario = t.IdUsuario,
                NombreVeterinario = t.NombreVeterinario,
                IdConsulta = t.IdConsulta,
                Nombre = t.Nombre,
                Descripcion = t.Descripcion,
                FechaInicio = t.FechaInicio,
                FechaFin = t.FechaFin,
                Estado = t.Estado,
                Seguimientos = seguimientos
            };
        }

        private static SeguimientoDto MapSeguimientoViewToDto(SeguimientoView s)
        {
            return new SeguimientoDto
            {
                Id = s.Id,
                IdTratamiento = s.IdTratamiento,
                IdUsuario = s.IdUsuario,
                NombreUsuario = s.NombreUsuario,
                Comentario = s.Comentario,
                Fecha = s.Fecha
            };
        }
    }
}