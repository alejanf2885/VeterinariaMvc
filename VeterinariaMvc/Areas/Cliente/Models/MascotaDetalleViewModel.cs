using VeterinariaMvc.Dtos.Clinica;
using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Dtos.Tratamiento;

namespace VeterinariaMvc.Areas.Cliente.Models
{
    public class MascotaDetalleViewModel
    {
        public List<ClinicaDto> Clinicas { get; set; }

        public MascotaDetalleDto Mascota { get; set; }

        public List<TratamientoDto> Tratamientos { get; set; } = new();
    }
}
