using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VeterinariaMvc.Areas.Veterinario.Models;
using VeterinariaMvc.Models.Plantillas;
using VeterinariaMvc.Repositories.Plantillas;

namespace VeterinariaMvc.Services.Plantillas
{
    public class PlantillaService : IPlantillaService
    {
        private readonly IPlantillaRepository _repo;

        public PlantillaService(IPlantillaRepository repo)
        {
            _repo = repo;
        }

        public Task<List<Plantilla>> GetPlantillasPorClinicaAsync(int idClinica)
        {
            return _repo.GetPlantillasPorClinicaAsync(idClinica);
        }

        public Task<Plantilla?> GetPlantillaByIdAsync(int idPlantilla)
        {
            return _repo.GetPlantillaByIdAsync(idPlantilla);
        }

        public async Task<int> CrearPlantillaCompletaAsync(CrearPlantillaViewModel model, int idClinica)
        {
            var plantilla = new Plantilla
            {
                Nombre = model.Nombre,
                Activa = model.Activa,
                IdClinica = idClinica
            };

            await _repo.CrearPlantillaAsync(plantilla);

            if (!string.IsNullOrWhiteSpace(model.SeccionesJson))
            {
                var opcionesJson = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var secciones = JsonSerializer.Deserialize<List<SeccionJsonModel>>(model.SeccionesJson, opcionesJson);

                if (secciones != null)
                {
                    foreach (var sec in secciones)
                    {
                        var nuevaSeccion = new PlantillaSeccion
                        {
                            IdPlantilla = plantilla.Id,
                            Nombre = sec.Nombre,
                            Orden = sec.Orden
                        };
                        await _repo.CrearPlantillaSeccionAsync(nuevaSeccion);

                        if (sec.Campos != null)
                        {
                            foreach (var camp in sec.Campos)
                            {
                                var nuevoCampo = new PlantillaCampo
                                {
                                    IdSeccion = nuevaSeccion.Id,
                                    Nombre = camp.Nombre,
                                    TipoDato = camp.TipoDato,
                                    Obligatorio = camp.Obligatorio,
                                    Orden = camp.Orden
                                };
                                await _repo.CrearPlantillaCampoAsync(nuevoCampo);

                                if (camp.Opciones != null && camp.Opciones.Count > 0)
                                {
                                    foreach (var opc in camp.Opciones)
                                    {
                                        if (!string.IsNullOrWhiteSpace(opc))
                                        {
                                            var nuevaOpcion = new PlantillaOpcion
                                            {
                                                IdCampo = nuevoCampo.Id,
                                                Valor = opc
                                            };
                                            await _repo.CrearPlantillaOpcionAsync(nuevaOpcion);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return plantilla.Id;
        }

        public async Task<PlantillaDetalleViewModel?> GetPlantillaDetalleAsync(int idPlantilla, int idClinica)
        {
            var plantilla = await _repo.GetPlantillaByIdAsync(idPlantilla);
            if (plantilla == null || plantilla.IdClinica != idClinica)
                return null;

            var secciones = await _repo.GetSeccionesByPlantillaAsync(idPlantilla);
            var idsSeccion = secciones.Select(s => s.Id).ToList();
            var campos = idsSeccion.Count > 0 
                ? await _repo.GetCamposBySeccionesAsync(idsSeccion) 
                : new List<PlantillaCampo>();

            var idsCampo = campos.Select(c => c.Id).ToList();
            var opciones = idsCampo.Count > 0 
                ? await _repo.GetOpcionesByCamposAsync(idsCampo) 
                : new List<PlantillaOpcion>();

            var vm = new PlantillaDetalleViewModel
            {
                Id = plantilla.Id,
                Nombre = plantilla.Nombre,
                Activa = plantilla.Activa ?? false
            };

            foreach (var sec in secciones.OrderBy(s => s.Orden ?? int.MaxValue))
            {
                var secVm = new PlantillaSeccionDetalle
                {
                    Id = sec.Id,
                    Nombre = sec.Nombre,
                    Orden = sec.Orden
                };

                var camposSec = campos.Where(c => c.IdSeccion == sec.Id)
                                      .OrderBy(c => c.Orden ?? int.MaxValue);

                foreach (var camp in camposSec)
                {
                    var campVm = new PlantillaCampoDetalle
                    {
                        Id = camp.Id,
                        Nombre = camp.Nombre,
                        TipoDato = camp.TipoDato,
                        Obligatorio = camp.Obligatorio,
                        Orden = camp.Orden,
                        Opciones = opciones
                            .Where(o => o.IdCampo == camp.Id)
                            .Select(o => o.Valor)
                            .ToList()
                    };

                    secVm.Campos.Add(campVm);
                }

                vm.Secciones.Add(secVm);
            }

            return vm;
        }

        public async Task<bool> UpdatePlantillaBasicaAsync(int idPlantilla, int idClinica, string nombre, bool activa)
        {
            var plantilla = await _repo.GetPlantillaByIdAsync(idPlantilla);
            if (plantilla == null || plantilla.IdClinica != idClinica)
                return false;

            plantilla.Nombre = nombre;
            plantilla.Activa = activa;
            return await _repo.UpdatePlantillaAsync(plantilla);
        }

        public Task<bool> DeletePlantillaAsync(int idPlantilla, int idClinica)
        {
            return _repo.DeletePlantillaCompletaAsync(idPlantilla, idClinica);
        }
    }
}

