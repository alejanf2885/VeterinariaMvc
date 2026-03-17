using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models.Plantillas
{
    [Table("PLANTILLA_OPCIONES")]
    public class PlantillaOpcion
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("ID_CAMPO")]
        public int IdCampo { get; set; }

        [Required]
        [StringLength(255)]
        [Column("VALOR")]
        public string Valor { get; set; }
    }
}
