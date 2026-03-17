using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models.Plantillas
{
    [Table("PLANTILLA_SECCION")]
    public class PlantillaSeccion
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("ID_PLANTILLA")]
        public int IdPlantilla { get; set; }

        [Required]
        [StringLength(150)]
        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("ORDEN")]
        public int? Orden { get; set; }
    }
}
