using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Models;

namespace VeterinariaMvc.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

    }
}