using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VeterinariaMvc.Areas.Veterinario.Models
{
    public class CrearPlantillaViewModel
    {
        [Required(ErrorMessage = "El nombre de la plantilla es obligatorio")]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        public bool Activa { get; set; } = true;

        public string SeccionesJson { get; set; } = "[]";
    }

    public class SeccionJsonModel
    {
        public string Nombre { get; set; } = string.Empty;
        public int Orden { get; set; }
        public List<CampoJsonModel> Campos { get; set; } = new List<CampoJsonModel>();
    }

    public class CampoJsonModel
    {
        public string Nombre { get; set; } = string.Empty;
        public string TipoDato { get; set; } = "Texto";
        public bool Obligatorio { get; set; }
        public int Orden { get; set; }
        public List<string> Opciones { get; set; } = new List<string>();
    }
}
