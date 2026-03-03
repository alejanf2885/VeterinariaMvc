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

            bool esDueno = (mascotaDetalle.IdUsuario == usuario.Id);

            if (mascotaDetalle == null) return null;

            bool esVeterinarioAutorizado = false;
            if(usuario.IdRol == (int)Roles.Veterinario)
            {
                int? idClinicaVeterinario = await this._clinicaService.ObtenerIdClinicaDeUsuarioAsync(usuario.Id);
                if(idClinicaVeterinario.HasValue && idClinicaVeterinario.Value == mascotaDetalle.IdClinica)
                {
                    esVeterinarioAutorizado = true;
                }
            }

            if(!esDueno && !esVeterinarioAutorizado)
            {
                return null;
            }

            MascotaDetalleDto mascotaDetalleDto = mascotaDetalle.ToDetalleDto();

            return mascotaDetalleDto;
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
