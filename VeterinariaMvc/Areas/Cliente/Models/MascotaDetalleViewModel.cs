using VeterinariaMvc.Dtos.Clinica;
using VeterinariaMvc.Dtos.Mascota;

namespace VeterinariaMvc.Areas.Cliente.Models
{
    public class MascotaDetalleViewModel
    {
        public List<ClinicaDto> Clinicas { get; set; }

        public MascotaDetalleDto Mascota { get; set; }
    }
}
