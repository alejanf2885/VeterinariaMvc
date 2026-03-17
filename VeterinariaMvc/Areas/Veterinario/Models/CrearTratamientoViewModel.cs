using System;
using System.ComponentModel.DataAnnotations;

namespace VeterinariaMvc.Areas.Veterinario.Models
{
    public class CrearTratamientoViewModel
    {
        [Required]
        public int IdConsulta { get; set; }

        [Required]
        public int IdMascota { get; set; }

        public string NombreMascota { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaFin { get; set; }

        // Necesitamos también el id interno de la tabla VETERINARIO
        [Required]
        public int IdVeterinario { get; set; }
    }
}
