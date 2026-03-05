using System.ComponentModel.DataAnnotations;
using VeterinariaMvc.Models.Enums;

namespace VeterinariaMvc.Dtos.Auth
{
    public class RegisterDto
    {

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Debes confirmar la contraseña.")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarPassword { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [MaxLength(20)]
        public string? Telefono { get; set; } 

        public IFormFile? Imagen { get; set; }

        public Roles Rol { get; set; } = Roles.Usuario;
    }
}