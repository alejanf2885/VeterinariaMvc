using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VeterinariaMvc.Areas.Admin.Models
{
    public class RegistroClinicaViewModel
    {
        [Required(ErrorMessage = "El email de contacto es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de la clínica es obligatorio")]
        public string NombreClinica { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; } = string.Empty;

        public string? Telefono { get; set; }

        [Required(ErrorMessage = "La hora de apertura es necesaria")]
        public string HoraAperturaStr { get; set; } = "09:00";

        [Required(ErrorMessage = "La hora de cierre es necesaria")]
        public string HoraCierreStr { get; set; } = "18:00";

        [BindNever]
        public TimeSpan HoraApertura => TimeSpan.TryParse(HoraAperturaStr, out var hA) ? hA : new TimeSpan(9, 0, 0);

        [BindNever]
        public TimeSpan HoraCierre => TimeSpan.TryParse(HoraCierreStr, out var hC) ? hC : new TimeSpan(18, 0, 0);

        [Required(ErrorMessage = "La duración es obligatoria")]
        [Range(15, 120, ErrorMessage = "Entre 15 y 120 minutos")]
        public int DuracionCita { get; set; }

        public IFormFile? Logo { get; set; }
    }
}