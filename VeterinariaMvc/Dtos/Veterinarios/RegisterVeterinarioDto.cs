using System.ComponentModel.DataAnnotations;
using VeterinariaMvc.Models.Enums;

namespace VeterinariaMvc.Dtos.Veterinarios
{
    public class RegisterVeterinarioDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo no válido")]
        public string Email { get; set; } = null!;

        [Phone(ErrorMessage = "Formato de teléfono no válido")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirma tu contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarPassword { get; set; } = null!;

        public IFormFile? Imagen { get; set; }


        [Display(Name = "Número de Colegiado")]
        public string? NumeroColegiado { get; set; }

        [Required]
        public int IdClinica { get; set; }

        [Required]
        public Roles Rol { get; set; } = Roles.Veterinario;
    }
}
