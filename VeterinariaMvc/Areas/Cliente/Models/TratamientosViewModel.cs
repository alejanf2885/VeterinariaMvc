using VeterinariaMvc.Dtos.Tratamiento;

namespace VeterinariaMvc.Areas.Cliente.Models
{
    public class TratamientosViewModel
    {
        public List<TratamientoDto> Tratamientos { get; set; } = new();
        public string? FiltroMascota { get; set; }
    }
}
