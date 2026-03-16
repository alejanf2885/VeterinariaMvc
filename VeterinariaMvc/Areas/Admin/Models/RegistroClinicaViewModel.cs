using System.ComponentModel.DataAnnotations;

namespace VeterinariaMvc.Areas.Admin.Models
{
    public class RegistroClinicaViewModel
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "El nombre de la clínica es obligatorio")]
        public string NombreClinica { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; }

        public string Telefono { get; set; }

        [Required(ErrorMessage = "La hora de apertura es necesaria")]
        public TimeSpan HoraApertura { get; set; }

        [Required(ErrorMessage = "La hora de cierre es necesaria")]
        public TimeSpan HoraCierre { get; set; }

        [Required]
        [Range(15, 120, ErrorMessage = "La duración debe ser entre 15 y 120 minutos")]
        public int DuracionCita { get; set; }
    }
}