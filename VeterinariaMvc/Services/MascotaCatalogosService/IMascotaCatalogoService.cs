using VeterinariaMvc.Areas.Cliente.Models;

namespace VeterinariaMvc.Services.MascotaCatalogosService
{
    public interface IMascotaCatalogoService
    {
        Task<CatalogosMascotaViewModels> GetCatalogoMascotasAsync();
    }
}
