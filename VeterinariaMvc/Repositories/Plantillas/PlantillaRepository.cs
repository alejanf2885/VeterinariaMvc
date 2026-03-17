using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Data;
using VeterinariaMvc.Models.Plantillas;

namespace VeterinariaMvc.Repositories.Plantillas
{
    public class PlantillaRepository : IPlantillaRepository
    {
        private readonly Context _context;

        public PlantillaRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<Plantilla>> GetPlantillasPorClinicaAsync(int idClinica)
        {
            return await _context.Plantillas
                .Where(p => p.IdClinica == idClinica)
                .ToListAsync();
        }

        public async Task<Plantilla?> GetPlantillaByIdAsync(int idPlantilla)
        {
            return await _context.Plantillas
                .FirstOrDefaultAsync(p => p.Id == idPlantilla);
        }

        public async Task<int> CrearPlantillaAsync(Plantilla plantilla)
        {
            _context.Plantillas.Add(plantilla);
            await _context.SaveChangesAsync();
            return plantilla.Id;
        }

        public async Task<int> CrearPlantillaSeccionAsync(PlantillaSeccion seccion)
        {
            _context.PlantillasSecciones.Add(seccion);
            await _context.SaveChangesAsync();
            return seccion.Id;
        }

        public async Task<int> CrearPlantillaCampoAsync(PlantillaCampo campo)
        {
            _context.PlantillasCampos.Add(campo);
            await _context.SaveChangesAsync();
            return campo.Id;
        }

        public async Task<int> CrearPlantillaOpcionAsync(PlantillaOpcion opcion)
        {
            _context.PlantillasOpciones.Add(opcion);
            await _context.SaveChangesAsync();
            return opcion.Id;
        }

        public async Task<List<PlantillaSeccion>> GetSeccionesByPlantillaAsync(int idPlantilla)
        {
            return await _context.PlantillasSecciones
                .Where(s => s.IdPlantilla == idPlantilla)
                .OrderBy(s => s.Orden)
                .ToListAsync();
        }

        public async Task<List<PlantillaCampo>> GetCamposBySeccionesAsync(List<int> idsSeccion)
        {
            return await _context.PlantillasCampos
                .Where(c => idsSeccion.Contains(c.IdSeccion))
                .OrderBy(c => c.Orden)
                .ToListAsync();
        }

        public async Task<List<PlantillaOpcion>> GetOpcionesByCamposAsync(List<int> idsCampo)
        {
            return await _context.PlantillasOpciones
                .Where(o => idsCampo.Contains(o.IdCampo))
                .ToListAsync();
        }

        public async Task<bool> UpdatePlantillaAsync(Plantilla plantilla)
        {
            _context.Plantillas.Update(plantilla);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePlantillaCompletaAsync(int idPlantilla, int idClinica)
        {
            // seguridad extra: solo si pertenece a la clínica
            var plantilla = await _context.Plantillas
                .FirstOrDefaultAsync(p => p.Id == idPlantilla && p.IdClinica == idClinica);

            if (plantilla == null)
                return false;

            var secciones = await _context.PlantillasSecciones
                .Where(s => s.IdPlantilla == idPlantilla)
                .ToListAsync();

            var idsSeccion = secciones.Select(s => s.Id).ToList();
            var campos = await _context.PlantillasCampos
                .Where(c => idsSeccion.Contains(c.IdSeccion))
                .ToListAsync();

            var idsCampo = campos.Select(c => c.Id).ToList();
            var opciones = await _context.PlantillasOpciones
                .Where(o => idsCampo.Contains(o.IdCampo))
                .ToListAsync();

            _context.PlantillasOpciones.RemoveRange(opciones);
            _context.PlantillasCampos.RemoveRange(campos);
            _context.PlantillasSecciones.RemoveRange(secciones);
            _context.Plantillas.Remove(plantilla);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
