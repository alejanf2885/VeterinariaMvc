using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Data;
using VeterinariaMvc.Models;
using VeterinariaMvc.Models.Auth;

namespace VeterinariaMvc.Repositories.Auth
{
    public class AuthUsuarioRepository : IAuthUsuarioRepository
    {

        private Context context;

        public AuthUsuarioRepository(Context context)
        {
            this.context = context;
        }


        public async Task<bool> ExisteEmailAsync(string email)
        {
            var consulta = from datos in this.context.AuthUsuarios
                           where datos.Email.Equals(email)
                           select datos;

            bool existe = await consulta.AnyAsync();

            return existe;
        }

        public async Task<AuthUsuario?> ObtenerPorEmailAsync(string email)
        {
            var consulta = from datos in this.context.AuthUsuarios
                           where datos.Email.Equals(email)
                           select datos;

            AuthUsuario usuario = await consulta.FirstOrDefaultAsync();

            return usuario;
        }
    }
}
