
using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Data;
using VeterinariaMvc.Enums;
using VeterinariaMvc.Models;
using VeterinariaMvc.Models.Enums;

namespace VeterinariaMvc.Repositories.Clinica
{
    public class VeterinarioRepository : IVeterinarioRepository
    {
        private Context context;
        public VeterinarioRepository(Context context)
        {
            this.context = context;
        }

        

        public async Task<int?> ObtenerIdClinicaDeUsuarioAsync(int idUsuario)
        {
            var consulta = from datos in this.context.Veterinarios
                           where datos.IdUsuario == idUsuario
                           select datos.IdClinica;

            int idClinica = await consulta.FirstOrDefaultAsync();
            return idClinica;
        }
        public async Task<bool> RegistrarVeterinarioAsync(int idUsuario, int idClinica, string? numeroColegiado)
        {
            try
            {
                Veterinario veterinario = new Veterinario
                {
                    IdUsuario = idUsuario,
                    IdClinica = idClinica,
                    NumeroColegiado = numeroColegiado
                };

                await this.context.Veterinarios.AddAsync(veterinario);
                await this.context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EliminarVeterinarioAsync(int idUsuario, int idClinica)
        {
            var veterinario = await this.context.Veterinarios
                .FirstOrDefaultAsync(v => v.IdUsuario == idUsuario && v.IdClinica == idClinica);

            if (veterinario == null)
                return false;

            try
            {
                
                this.context.Veterinarios.Remove(veterinario);
                await this.context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
