using System.Collections.Generic;
using System.Threading.Tasks;
using VeterinariaMvc.Models.Plantillas;

namespace VeterinariaMvc.Repositories.Plantillas
{
    public interface IPlantillaRepository
    {
        Task<List<Plantilla>> GetPlantillasPorClinicaAsync(int idClinica);
        Task<Plantilla?> GetPlantillaByIdAsync(int idPlantilla);
        Task<int> CrearPlantillaAsync(Plantilla plantilla);

        Task<int> CrearPlantillaSeccionAsync(PlantillaSeccion seccion);
        Task<int> CrearPlantillaCampoAsync(PlantillaCampo campo);
        Task<int> CrearPlantillaOpcionAsync(PlantillaOpcion opcion);

        Task<List<PlantillaSeccion>> GetSeccionesByPlantillaAsync(int idPlantilla);
        Task<List<PlantillaCampo>> GetCamposBySeccionesAsync(List<int> idsSeccion);
        Task<List<PlantillaOpcion>> GetOpcionesByCamposAsync(List<int> idsCampo);

        Task<bool> UpdatePlantillaAsync(Plantilla plantilla);
        Task<bool> DeletePlantillaCompletaAsync(int idPlantilla, int idClinica);
    }
}