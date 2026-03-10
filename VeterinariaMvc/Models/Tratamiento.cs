using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models
{
    [Table("TRATAMIENTO")]
    public class Tratamiento
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("ID_MASCOTA")]
        public int IdMascota { get; set; }

        [Column("ID_VETERINARIO")]
        public int IdVeterinario { get; set; }

        [Column("ID_CONSULTA")]
        public int? IdConsulta { get; set; }

        [Column("NOMBRE")]
        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; }

        [Column("DESCRIPCION")]
        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [Column("FECHA_INICIO")]
        public DateTime FechaInicio { get; set; }

        [Column("FECHA_FIN")]
        public DateTime? FechaFin { get; set; }

        [Column("ESTADO")]
        [MaxLength(30)]
        public string? Estado { get; set; }
    }
}
