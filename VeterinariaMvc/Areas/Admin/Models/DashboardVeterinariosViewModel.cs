using System.Collections.Generic;
using VeterinariaMvc.Dtos.Consultas.VeterinariaMvc.Dtos.Consulta;

namespace VeterinariaMvc.Areas.Admin.Models
{
    public class DashboardVeterinariosViewModel
    {
        public List<DashboardCitaSinVeterinario> CitasSinVeterinario { get; set; } = new();
        public List<DashboardCitaVeterinario> CitasPorVeterinario { get; set; } = new();

        // Estadísticas rápidas para las tarjetas del dashboard
        public int TotalConsultasHoy { get; set; }
        public int TotalMascotasRegistradas { get; set; }
        public int TotalCitasSinVeterinario { get; set; }
    }
}

