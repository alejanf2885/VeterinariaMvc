using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models
{
    [Table("SEGUIMIENTO_TRATAMIENTO")]
    public class SeguimientoTratamiento
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("ID_TRATAMIENTO")]
        public int IdTratamiento { get; set; }

        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [Column("COMENTARIO")]
        [Required]
        public string Comentario { get; set; }

        [Column("FECHA")]
        public DateTime Fecha { get; set; }
    }
}
