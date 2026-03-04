using VeterinariaMvc.Dtos.Clinica;
using VeterinariaMvc.Models;

namespace VeterinariaMvc.Mappers
{
    public static class ClinicaMapper
    {
        public static ClinicaDto ToClinicaDto(this Clinica clinica)
        {
            if(clinica == null) return null;

            return new ClinicaDto
            {
                Id = clinica.Id,
                Nombre = clinica.Nombre,
                Direccion = clinica.Direccion,
                Telefono = clinica.Telefono,
                Imagen = clinica.Imagen,
                Email = clinica.Email
            };
        }

    }
}
