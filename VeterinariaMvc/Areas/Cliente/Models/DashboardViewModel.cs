using VeterinariaMvc.Dtos.Consultas.VeterinariaMvc.Dtos.Consulta;
using VeterinariaMvc.Dtos.Mascota;
using VeterinariaMvc.Dtos.Session;

namespace VeterinariaMvc.Areas.Cliente.Models
{
    public class DashboardViewModel
    {

        public UsuarioSessionDto usuario { get; set; }
        public List<MascotaResumenDto> Mascotas { get; set; }
        public List<ConsultaResumen> Consultas { get; set; }
    }
}
