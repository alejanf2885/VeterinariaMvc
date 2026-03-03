using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models
{
    [Table("VETERINARIO")]
    public class Veterinario
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [Column("ID_CLINICA")]
        public int IdClinica { get; set; }

        [Column("NUMERO_COLEGIADO")]
        public string NumeroColegiado { get; set; }
    }
}
