using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Data;
using VeterinariaMvc.Models;

namespace VeterinariaMvc.Repositories.UsuarioRepository
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private Context context;

        public UsuarioRepository(Context context)
        {
            this.context = context;
        }
        
        public async Task<Usuario?> ObtenerPorEmailAsync(string email)
        {

            var consulta = from datos in this.context.Usuarios
                           where datos.Email.Equals(email)
                           select datos;

            Usuario usuario = await consulta.FirstOrDefaultAsync();

            return usuario;
        }
    }
}
