using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models.Seguimientos
{
    [Table("VW_SEGUIMIENTOS_POR_TRATAMIENTO")]
    public class SeguimientoView
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("IdTratamiento")]
        public int IdTratamiento { get; set; }

        [Column("IdUsuario")]
        public int IdUsuario { get; set; }

        [Column("NombreUsuario")]
        public string NombreUsuario { get; set; }

        [Column("Comentario")]
        public string Comentario { get; set; }

        [Column("Fecha")]
        public DateTime Fecha { get; set; }
    }
}