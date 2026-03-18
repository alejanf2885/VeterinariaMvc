using VeterinariaMvc.Dtos.Tratamiento;
using VeterinariaMvc.Models.Seguimientos;
using VeterinariaMvc.Models.Tratamientos;
using VeterinariaMvc.Repositories.Consulta;
using VeterinariaMvc.Repositories.Tratamientos;

namespace VeterinariaMvc.Services.Tratamientos
{
    public class TratamientoService : ITratamientoService
    {
        private readonly ITratamientoRepository _tratamientoRepository;
        private readonly IConsultaRepository _consultaRepository;

        public TratamientoService(ITratamientoRepository tratamientoRepository, IConsultaRepository consultaRepository)
        {
            _tratamientoRepository = tratamientoRepository;
            _consultaRepository = consultaRepository;
        }

        public async Task<List<TratamientoDto>> GetTratamientosPorMascotaAsync(int idMascota)
        {
            List<TratamientoView> tratamientos = await _tratamientoRepository.GetTratamientosPorMascotaAsync(idMascota);
            return await MapTratamientosAListaDto(tratamientos);
        }

        public async Task<List<TratamientoDto>> GetTratamientosPorUsuarioAsync(int idUsuario)
        {
            List<TratamientoView> tratamientos = await _tratamientoRepository.GetTratamientosPorUsuarioAsync(idUsuario);
            return await MapTratamientosAListaDto(tratamientos);
        }

        public async Task<TratamientoDto?> GetTratamientoDetalleAsync(int idTratamiento)
        {
            TratamientoView tratamiento = await _tratamientoRepository.GetTratamientoDetalleAsync(idTratamiento);

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

        public async Task<bool> CrearTratamientoAsync(
            int idMascota,
            int idVeterinario,
            int idConsulta,
            string nombre,
            string? descripcion,
            DateTime fechaInicio,
            DateTime? fechaFin)
        {
            bool creado = await _tratamientoRepository.CrearTratamientoAsync(
                idMascota,
                idVeterinario,
                idConsulta,
                nombre,
                descripcion,
                fechaInicio,
                fechaFin);

            if (creado && idConsulta > 0)
            {
                await _consultaRepository.ActualizarEstadoConsultaAsync(idConsulta, "EN CURSO");
            }

            return creado;
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