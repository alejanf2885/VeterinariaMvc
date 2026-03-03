using VeterinariaMvc.Areas.Cliente.Models;
using VeterinariaMvc.Repositories.EspecieRepository;
using VeterinariaMvc.Repositories.RazaRepository;

namespace VeterinariaMvc.Services.MascotaCatalogosService
{
    public class MascotaCatalogoService : IMascotaCatalogoService
    {
        private  IEspecieRepository _especieRepository;
        private  IRazaRepository _razaRepository;

        public MascotaCatalogoService(IEspecieRepository especieRepository, IRazaRepository razaRepository)
        {
            _especieRepository = especieRepository;
            _razaRepository = razaRepository;
        }

        public async Task<CatalogosMascotaViewModels> GetCatalogoMascotasAsync()
        {
            var especies = await _especieRepository.GetEspeciesAsync();
            var razas = await _razaRepository.GetRazasAsync();

            CatalogosMascotaViewModels catalogo = new CatalogosMascotaViewModels
            {
                Especies = especies,
                Razas = razas
            };
            return catalogo;
        }
    }
}
