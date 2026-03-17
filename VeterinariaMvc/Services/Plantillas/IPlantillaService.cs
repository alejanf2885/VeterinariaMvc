using System.Collections.Generic;
using System.Threading.Tasks;
using VeterinariaMvc.Areas.Veterinario.Models;
using VeterinariaMvc.Models.Plantillas;

namespace VeterinariaMvc.Services.Plantillas
{
    public interface IPlantillaService
    {
        Task<List<Plantilla>> GetPlantillasPorClinicaAsync(int idClinica);
        Task<Plantilla?> GetPlantillaByIdAsync(int idPlantilla);
        Task<int> CrearPlantillaCompletaAsync(CrearPlantillaViewModel model, int idClinica);
        Task<PlantillaDetalleViewModel?> GetPlantillaDetalleAsync(int idPlantilla, int idClinica);
        Task<bool> UpdatePlantillaBasicaAsync(int idPlantilla, int idClinica, string nombre, bool activa);
        Task<bool> DeletePlantillaAsync(int idPlantilla, int idClinica);

        Task<int> CrearOActualizarFichaConsultaDesdePlantillaAsync(int idConsulta, int idPlantilla, IDictionary<int, string?> valoresTexto, IDictionary<int, decimal?> valoresNumero, IDictionary<int, System.DateTime?> valoresFecha, IDictionary<int, bool?> valoresBooleano);
        Task<List<FichaValor>> GetValoresFichaPorConsultaAsync(int idConsulta);
    }
}

