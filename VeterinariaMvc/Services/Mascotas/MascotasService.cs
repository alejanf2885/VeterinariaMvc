using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Enums;
using VeterinariaMvc.Mappers;
using VeterinariaMvc.Models;
using VeterinariaMvc.Repositories.MascotasRepository;
using VeterinariaMvc.Services.FileStorage;
using VeterinariaMvc.Services.Imagenes;

namespace VeterinariaMvc.Services.Mascotas
{
    public class MascotasService : IMascotasService
    {
        private readonly IMascotasRepository _mascotasRepo;
        private readonly IImagenService _imagenService;

        public MascotasService(IMascotasRepository mascotasRepo, IImagenService imagenService)
        {
            _mascotasRepo = mascotasRepo;
            _imagenService = imagenService;
        }

        public async Task<MascotaDetalleDto?> GetMascotaPorIdAsync(int idMascota)
        {
            MascotaDetalle mascotaDetalle = await _mascotasRepo.GetMascotaPorIdAsync(idMascota);
            if (mascotaDetalle == null) return null;

            return mascotaDetalle.ToDetalleDto();
        }

        public async Task<MascotaEditDto?> GetMascotaParaEditarAsync(int idMascota)
        {
            Mascota? mascota = await _mascotasRepo.GetMascotaEntityPorIdAsync(idMascota);
            if (mascota == null) return null;

            MascotaEditDto dto = new MascotaEditDto();
            dto.Id = mascota.Id;
            dto.Nombre = mascota.Nombre;
            dto.IdEspecie = mascota.IdEspecie;
            dto.IdRaza = mascota.IdRaza;
            dto.Sexo = mascota.Sexo;
            dto.FechaNacimiento = mascota.FechaNacimiento;
            dto.PesoActual = mascota.PesoActual;
            dto.ImagenActual = string.IsNullOrEmpty(mascota.Imagen)
                ? "/images/mascotas/default-avatar.png"
                : mascota.Imagen;

            return dto;
        }

        public async Task<bool> EditarMascotaAsync(MascotaEditDto dto)
        {
            Mascota? entidad = await _mascotasRepo.GetMascotaEntityPorIdAsync(dto.Id);
            if (entidad == null) return false;

            entidad.Nombre = dto.Nombre;
            entidad.IdEspecie = dto.IdEspecie;
            entidad.IdRaza = dto.IdRaza;
            entidad.Sexo = dto.Sexo;
            entidad.FechaNacimiento = dto.FechaNacimiento;
            entidad.PesoActual = dto.PesoActual;

            if (dto.NuevaImagen != null)
            {
                // GUARDAMOS LA RUTA ANTIGUA
                string? rutaImagenVieja = entidad.Imagen;

                // SUBIMOS LA NUEVA
                string nuevaRuta = await _imagenService.SubirImagenAsync(
                    dto.NuevaImagen,
                    CarpetaDestino.Mascotas,
                    1000
                );

                // ACTUALIZAMOS LA ENTIDAD
                entidad.Imagen = nuevaRuta;

                // BORRAMOS LA ANTIGUA DEL DISCO
                if (!string.IsNullOrEmpty(rutaImagenVieja))
                {
                    await _imagenService.BorrarImagenAsync(rutaImagenVieja);
                }
            }

            return await _mascotasRepo.ActualizarMascotaAsync(entidad);
        }

        public async Task<bool> DesactivarMascotaAsync(int idMascota)
        {
            return await _mascotasRepo.EliminarMascotaAsync(idMascota);
        }

        public async Task<bool> AsignarClinicaAMascotaAsync(int idMascota, int idClinica)
        {
            return await _mascotasRepo.AsignarClinicaAMascotaAsync(idMascota, idClinica);
        }

        public async Task<List<MascotaResumenDto>> GetMascotasByUserAsync(int idUsuario)
        {
            return await _mascotasRepo.GetMascotaPorUsuarioAsync(idUsuario);
        }

        public async Task<int> RegistrarMascotaAsync(MascotaRegisterDto mascotaRegistrarDto, int idUsuario)
        {
            string rutaFoto = "/images/mascotas/default-avatar.png";

            if (mascotaRegistrarDto.Imagen != null)
            {
                rutaFoto = await _imagenService.SubirImagenAsync(mascotaRegistrarDto.Imagen, CarpetaDestino.Mascotas, 1000);
            }

            return await _mascotasRepo.RegistrarMascotaAsync(
                mascotaRegistrarDto.Nombre,
                mascotaRegistrarDto.IdEspecie,
                mascotaRegistrarDto.IdRaza,
                mascotaRegistrarDto.Sexo,
                mascotaRegistrarDto.FechaNacimiento,
                mascotaRegistrarDto.PesoActual,
                rutaFoto,
                idUsuario
            );
        }

        public async Task<List<MascotaDetalle>> ObtenerMascotasPorClinicaAsync(int idClinica)
        {
            return await this._mascotasRepo.ObtenerMascotasPorClinicaAsync(idClinica);
        }
    }
}