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
        [Display(Name = "Hora de apertura")]
        [RegularExpression(@"^([0-1]\d|2[0-3]):([0-5]\d)$", ErrorMessage = "Formato de hora inválido")]
        public string HoraAperturaStr { get; set; }

        [Required(ErrorMessage = "La hora de cierre es necesaria")]
        [Display(Name = "Hora de cierre")]
        [RegularExpression(@"^([0-1]\d|2[0-3]):([0-5]\d)$", ErrorMessage = "Formato de hora inválido")]
        public string HoraCierreStr { get; set; }

        public TimeSpan HoraApertura => TimeSpan.TryParse(HoraAperturaStr, out var hA) ? hA : TimeSpan.Zero;
        public TimeSpan HoraCierre => TimeSpan.TryParse(HoraCierreStr, out var hC) ? hC : TimeSpan.Zero;

        [Required]
        [Range(15, 120, ErrorMessage = "La duración debe ser entre 15 y 120 minutos")]
        public int DuracionCita { get; set; }

        public IFormFile? Logo { get; set; }
    }
}