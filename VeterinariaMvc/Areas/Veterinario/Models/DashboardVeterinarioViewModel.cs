using System.Collections.Generic;
using VeterinariaMvc.Dtos.Consultas;

namespace VeterinariaMvc.Areas.Veterinario.Models
{
    public class DashboardVeterinarioViewModel
    {
        public List<ConsultaVeterinarioDto> CitasAsignadas { get; set; } = new List<ConsultaVeterinarioDto>();
    }
}
