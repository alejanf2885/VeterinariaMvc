using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models.Tratamientos
{
    [Table("VW_TRATAMIENTOS")]
    public class TratamientoView
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("IdMascota")]
        public int IdMascota { get; set; }

        [Column("NombreMascota")]
        public string NombreMascota { get; set; }

        [Column("IdVeterinario")]
        public int IdVeterinario { get; set; }

        [Column("IdUsuario")]
        public int IdUsuario { get; set; }

        [Column("NombreVeterinario")]
        public string NombreVeterinario { get; set; }

        [Column("IdConsulta")]
        public int? IdConsulta { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("Descripcion")]
        public string? Descripcion { get; set; }

        [Column("FechaInicio")]
        public DateTime FechaInicio { get; set; }

        [Column("FechaFin")]
        public DateTime? FechaFin { get; set; }

        [Column("Estado")]
        public string Estado { get; set; }
    }
}