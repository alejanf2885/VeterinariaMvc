using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models.Plantillas
{
    [Table("PLANTILLA_CAMPO")]
    public class PlantillaCampo
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("ID_SECCION")]
        public int IdSeccion { get; set; }

        [Required]
        [StringLength(150)]
        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [StringLength(50)]
        [Column("TIPO_DATO")]
        public string? TipoDato { get; set; }

        [Column("OBLIGATORIO")]
        public bool? Obligatorio { get; set; }

        [Column("ORDEN")]
        public int? Orden { get; set; }
    }
}
