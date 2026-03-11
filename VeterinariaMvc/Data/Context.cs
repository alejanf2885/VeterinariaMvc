using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Dtos.Bloque;
using VeterinariaMvc.Dtos.Consultas;
using VeterinariaMvc.Dtos.Consultas.VeterinariaMvc.Dtos.Consulta;
using VeterinariaMvc.Dtos.Tratamiento;
using VeterinariaMvc.Models;
using VeterinariaMvc.Models.Auth;
using VeterinariaMvc.Models.Seguimientos;
using VeterinariaMvc.Models.Tratamientos;

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
        public DbSet<Tratamiento> Tratamientos { get; set; }
        public DbSet<SeguimientoTratamiento> SeguimientosTratamiento { get; set; }
        public DbSet<TratamientoView> TratamientosView { get; set; }
        public DbSet<SeguimientoView> SeguimientosView { get; set; }



    }
}