using System.ComponentModel.DataAnnotations;

namespace VeterinariaMvc.Dtos.Mascota
{
    public class MascotaEditDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        public int? IdEspecie { get; set; }
        public int? IdRaza { get; set; }

        public string? Sexo { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        [Range(0, 200)]
        public decimal? PesoActual { get; set; }

        // Imagen actual de la mascota (URL)
        public string? ImagenActual { get; set; }

        // Nueva imagen opcional que el usuario suba en Editar
        public IFormFile? NuevaImagen { get; set; }
    }
}

