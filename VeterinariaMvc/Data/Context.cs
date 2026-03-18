using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Dtos.Bloque;
using VeterinariaMvc.Dtos.Consultas;
using VeterinariaMvc.Dtos.Consultas.VeterinariaMvc.Dtos.Consulta;
using VeterinariaMvc.Dtos.Tratamiento;
using VeterinariaMvc.Models;
using VeterinariaMvc.Models.Auth;
using VeterinariaMvc.Models.Chats;
using VeterinariaMvc.Models.Clientes;
using VeterinariaMvc.Models.Seguimientos;
using VeterinariaMvc.Models.Tratamientos;
using VeterinariaMvc.Models.Plantillas;

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
        public DbSet<ChatConversacion> ChatConversaciones { get; set; }
        public DbSet<ChatMensaje> ChatMensajes { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<DashboardCitaSinVeterinario> CitasSinVeterinarioDashboard { get; set; }
        public DbSet<DashboardCitaVeterinario> CitasVeterinarioDashboard { get; set; }

        public DbSet<Plantilla> Plantillas { get; set; }
        public DbSet<PlantillaSeccion> PlantillasSecciones { get; set; }
        public DbSet<PlantillaCampo> PlantillasCampos { get; set; }
        public DbSet<PlantillaOpcion> PlantillasOpciones { get; set; }

        public DbSet<FichaConsulta> FichasConsulta { get; set; }
        public DbSet<FichaValor> FichasValores { get; set; }

    }
}