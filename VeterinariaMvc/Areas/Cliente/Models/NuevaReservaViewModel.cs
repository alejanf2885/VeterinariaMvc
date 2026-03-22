using System.ComponentModel.DataAnnotations;
using VeterinariaMvc.Dtos.Bloque;

namespace VeterinariaMvc.Areas.Cliente.Models
{
    public class NuevaReservaViewModel
    {
        public int IdMascota { get; set; }
        public int IdClinica { get; set; }

        public string? NombreMascota { get; set; }
        public string? NombreClinica { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaSeleccionada { get; set; }

        [Required(ErrorMessage = "Debes seleccionar una hora")]
        public int? IdBloque { get; set; }

        [Required(ErrorMessage = "El motivo es obligatorio")]
        [StringLength(255)]
        public string? Motivo { get; set; }

        public List<BloqueDisponibleDto> HorariosDisponibles { get; set; } = new List<BloqueDisponibleDto>();
    }
}