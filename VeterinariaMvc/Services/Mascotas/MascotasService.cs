using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Dtos.Session;
using VeterinariaMvc.Enums;
using VeterinariaMvc.Mappers;
using VeterinariaMvc.Models;
using VeterinariaMvc.Models.Enums;
using VeterinariaMvc.Repositories.MascotasRepository;
using VeterinariaMvc.Services.Clinica;
using VeterinariaMvc.Services.Imagenes;

namespace VeterinariaMvc.Services.Mascotas
{
    public class MascotasService : IMascotasService
    {
        private IMascotasRepository _mascotasRepo;
        private IImagenService _imagenService;
        private IClinicaService _clinicaService;

        public MascotasService(IMascotasRepository mascotasRepo, IImagenService imagenService, IClinicaService clinicaService)
        {
            this._mascotasRepo = mascotasRepo;
            this._imagenService = imagenService;
            this._clinicaService = clinicaService;
        }

        public async Task<MascotaDetalleDto?> GetMascotaPorIdAsync(int idMascota, UsuarioSessionDto usuario)
        {
            MascotaDetalle mascotaDetalle = await this._mascotasRepo.GetMascotaPorIdAsync(idMascota);
            if (mascotaDetalle == null) return null;

            bool esDueno = (mascotaDetalle.IdUsuario == usuario.Id);

            bool esVeterinarioAutorizado = false;
            if (usuario.IdRol == (int)Roles.Veterinario)
            {
                int? idClinicaVeterinario = await this._clinicaService.ObtenerIdClinicaDeUsuarioAsync(usuario.Id);
                if (idClinicaVeterinario.HasValue && idClinicaVeterinario.Value == mascotaDetalle.IdClinica)
                {
                    esVeterinarioAutorizado = true;
                }
            }

            if (!esDueno && !esVeterinarioAutorizado)
            {
                return null;
            }

            MascotaDetalleDto mascotaDetalleDto = mascotaDetalle.ToDetalleDto();

            return mascotaDetalleDto;
        }

        // Solo el dueño o un veterinario autorizado pueden obtener los datos para editar
        public async Task<MascotaEditDto?> GetMascotaParaEditarAsync(int idMascota, UsuarioSessionDto usuario)
        {
            MascotaDetalle mascotaDetalle = await this._mascotasRepo.GetMascotaPorIdAsync(idMascota);
            if (mascotaDetalle == null) return null;

            bool esDueno = (mascotaDetalle.IdUsuario == usuario.Id);
            bool esVeterinarioAutorizado = false;

            // Un veterinario está autorizado si pertenece a la misma clínica que la mascota
            if (usuario.IdRol == (int)Roles.Veterinario)
            {
                int? idClinicaVeterinario = await this._clinicaService.ObtenerIdClinicaDeUsuarioAsync(usuario.Id);
                if (idClinicaVeterinario.HasValue && idClinicaVeterinario.Value == mascotaDetalle.IdClinica)
                {
                    esVeterinarioAutorizado = true;
                }
            }

            if (!esDueno && !esVeterinarioAutorizado)
            {
                return null;
            }

            Mascota? entidad = await this._mascotasRepo.GetMascotaEntityPorIdAsync(idMascota);
            if (entidad == null) return null;

            MascotaEditDto dto = new MascotaEditDto();
            dto.Id = entidad.Id;
            dto.Nombre = entidad.Nombre;
            dto.IdEspecie = entidad.IdEspecie;
            dto.IdRaza = entidad.IdRaza;
            dto.Sexo = entidad.Sexo;
            dto.FechaNacimiento = entidad.FechaNacimiento;
            dto.PesoActual = entidad.PesoActual;

            return dto;

          
        }


        public async Task<bool> EditarMascotaAsync(MascotaEditDto dto, UsuarioSessionDto usuario)
        {
            MascotaDetalle mascotaDetalle = await this._mascotasRepo.GetMascotaPorIdAsync(dto.Id);
            if (mascotaDetalle == null) return false;

            bool esDueno = (mascotaDetalle.IdUsuario == usuario.Id);
            if (!esDueno) return false; // de momento solo el dueño puede editar

            Mascota? entidad = await this._mascotasRepo.GetMascotaEntityPorIdAsync(dto.Id);
            if (entidad == null) return false;

            entidad.Nombre = dto.Nombre;
            entidad.IdEspecie = dto.IdEspecie;
            entidad.IdRaza = dto.IdRaza;
            entidad.Sexo = dto.Sexo;
            entidad.FechaNacimiento = dto.FechaNacimiento;
            entidad.PesoActual = dto.PesoActual;

            return await this._mascotasRepo.ActualizarMascotaAsync(entidad);
        }

        public async Task<List<MascotaResumenDto>> GetMascotasByUserAsync(int idUsuario)
        {
            List<MascotaResumenDto> mascotas =
                await this._mascotasRepo.GetMascotaPorUsuarioAsync(idUsuario);

            return mascotas;
        }

        public async Task<int> RegistrarMascotaAsync(MascotaRegisterDto mascotaRegistrarDto, int idUsuario)
        {
            string rutaFoto = "/images/mascotas/default-avatar.png";

            if (mascotaRegistrarDto.Imagen != null)
            {
                rutaFoto = await
                    this._imagenService.SubirImagenAsync(mascotaRegistrarDto.Imagen, CarpetaDestino.Mascotas, 1000);
            }

            int idNuevo = await this._mascotasRepo.RegistrarMascotaAsync
                (mascotaRegistrarDto.Nombre,
                mascotaRegistrarDto.IdEspecie,
                mascotaRegistrarDto.IdRaza,
                mascotaRegistrarDto.Sexo,
                mascotaRegistrarDto.FechaNacimiento,
                mascotaRegistrarDto.PesoActual,
                rutaFoto,
                idUsuario
            );

            return idNuevo;
        }
    }
}
