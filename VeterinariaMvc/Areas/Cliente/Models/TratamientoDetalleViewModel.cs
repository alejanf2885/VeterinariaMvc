using VeterinariaMvc.Dtos.Tratamiento;
using VeterinariaMvc.Models.Tratamientos;

namespace VeterinariaMvc.Areas.Cliente.Models
{
    public class TratamientoDetalleViewModel
    {
        public TratamientoDto Tratamiento { get; set; }
        public string NuevoComentario { get; set; }

        
    }
}
