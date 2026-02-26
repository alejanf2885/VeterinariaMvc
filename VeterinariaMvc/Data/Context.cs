using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Models;
using VeterinariaMvc.Models.Auth;

namespace VeterinariaMvc.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<AuthUsuario> AuthUsuarios { get; set; }


    }
}