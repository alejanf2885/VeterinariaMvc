using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models.Plantillas
{
    [Table("PLANTILLA")]
    public class Plantilla
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("ACTIVA")]
        public bool? Activa { get; set; } = true;

        [Column("ID_CLINICA")]
        public int IdClinica { get; set; }
    }
}
