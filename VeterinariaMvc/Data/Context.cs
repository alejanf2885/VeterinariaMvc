using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Dtos.Bloque;
using VeterinariaMvc.Dtos.Consultas;
using VeterinariaMvc.Dtos.Consultas.VeterinariaMvc.Dtos.Consulta;
using VeterinariaMvc.Models;
using VeterinariaMvc.Models.Auth;

namespace VeterinariaMvc.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<MascotaDetalle> MascotasDetalles { get; set; }
        public DbSet<AuthUsuario> AuthUsuarios { get; set; }
        public DbSet<Especie> Especies { get; set; }
        public DbSet<Raza> Razas { get; set; }
        public DbSet<Clinica> Clinicas { get; set; }
        public DbSet<Veterinario> Veterinarios { get; set; }
        public DbSet<BloqueDisponibleDto> BloquesDisponibles { get; set; }
        public DbSet<ConsultaResumen> ConsultasResumidas { get; set; }
    }
}