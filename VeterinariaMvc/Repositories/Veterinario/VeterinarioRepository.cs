
using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Data;

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
    }
}
