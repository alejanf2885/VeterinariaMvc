using System.Collections.Generic;
using VeterinariaMvc.Dtos.Consultas;
using VeterinariaMvc.Models.Plantillas;

namespace VeterinariaMvc.Areas.Veterinario.Models
{
    public class DashboardVeterinarioViewModel
    {
        public List<ConsultaVeterinarioDto> CitasAsignadas { get; set; } = new List<ConsultaVeterinarioDto>();
		public List<Plantilla> PlantillasDisponibles { get; set; } = new List<Plantilla>();
    }
}
