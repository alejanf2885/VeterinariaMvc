using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Models;

namespace VeterinariaMvc.Mappers
{
    public static class MascotaDetalleMapper
    {
        public static MascotaDetalleDto ToDetalleDto(this MascotaDetalle entidad)
        {
            if (entidad == null) return null;

            return new MascotaDetalleDto
            {
                IdMascota = entidad.IdMascota,
                IdUsuario = entidad.IdUsuario,
                NombreMascota = entidad.NombreMascota,
                Especie = entidad.Especie,
                Raza = entidad.Raza,
                ImagenMascota = entidad.ImagenMascota,
                Peso = entidad.Peso,
                NombreClinica = entidad.NombreClinica,
                DireccionClinica = entidad.DireccionClinica,
                IdClinica = entidad.IdClinica ?? 0,
                EstadoEnClinica = entidad.EstadoEnClinica
            };
        }
    }
}