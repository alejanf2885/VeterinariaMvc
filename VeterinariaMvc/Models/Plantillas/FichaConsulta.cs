using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models.Plantillas
{
    [Table("FICHA_CONSULTA")]
    public class FichaConsulta
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("ID_CONSULTA")]
        public int IdConsulta { get; set; }

        [Column("ID_PLANTILLA")]
        public int? IdPlantilla { get; set; }

        [Column("OBSERVACIONES_GENERALES")]
        public string? ObservacionesGenerales { get; set; }
    }
}
