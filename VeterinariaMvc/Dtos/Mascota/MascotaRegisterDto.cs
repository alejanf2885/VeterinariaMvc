using System.ComponentModel.DataAnnotations;

namespace VeterinariaMvc.Dtos.Mascota
{
    public class MascotaRegisterDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public int IdCliente { get; set; }

        public int? IdEspecie { get; set; }
        public int? IdRaza { get; set; }

        public string? Sexo { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        [Range(0, 200)]
        public double? PesoActual { get; set; }

        public IFormFile? Imagen { get; set; }

    }
}
