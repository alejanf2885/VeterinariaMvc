using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Dtos.Session;
using VeterinariaMvc.Enums;
using VeterinariaMvc.Mappers;
using VeterinariaMvc.Models;
using VeterinariaMvc.Models.Enums;
using VeterinariaMvc.Repositories.MascotasRepository;
using VeterinariaMvc.Services.Clinica;
using VeterinariaMvc.Services.FileStorage;
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

            Mascota? mascota = await this._mascotasRepo.GetMascotaEntityPorIdAsync(idMascota);
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


        public async Task<bool> EditarMascotaAsync(MascotaEditDto dto, UsuarioSessionDto usuario)
        {
            MascotaDetalle mascotaDetalle = await this._mascotasRepo.GetMascotaPorIdAsync(dto.Id);
            if (mascotaDetalle == null) return false;

            bool esDueno = (mascotaDetalle.IdUsuario == usuario.Id);
            //if (!esDueno) return false; 
            bool esVeterinarioAutorizado = false;
            if (usuario.IdRol == (int)Roles.Veterinario)
            {
                int? idClinicaVeterinario = await this._clinicaService.ObtenerIdClinicaDeUsuarioAsync(usuario.Id);
                if (idClinicaVeterinario.HasValue && idClinicaVeterinario.Value == mascotaDetalle.IdClinica)
                {
                    esVeterinarioAutorizado = true;
                }
            }
            if(!esDueno && !esVeterinarioAutorizado) return false;

            Mascota? entidad = await this._mascotasRepo.GetMascotaEntityPorIdAsync(dto.Id);
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
                string nuevaRuta = await this._imagenService.SubirImagenAsync(
                    dto.NuevaImagen,
                    CarpetaDestino.Mascotas,
                    1000
                );

                // ACTUALIZAMOS LA ENTIDAD
                entidad.Imagen = nuevaRuta;

                // BORRAMOS LA ANTIGUA DEL DISCO
                if (!string.IsNullOrEmpty(rutaImagenVieja))
                {
                    await this._imagenService.BorrarImagenAsync(rutaImagenVieja);
                }
            }

            return await this._mascotasRepo.ActualizarMascotaAsync(entidad);
        }

        public async Task<bool> DesactivarMascotaAsync(int idMascota, UsuarioSessionDto usuario)
        {
            MascotaDetalle mascotaDetalle = await this._mascotasRepo.GetMascotaPorIdAsync(idMascota);
            if (mascotaDetalle == null) return false;

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

            if (!esDueno && !esVeterinarioAutorizado) return false;

            return await this._mascotasRepo.EliminarMascotaAsync(idMascota);
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
