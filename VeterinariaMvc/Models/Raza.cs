using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models
{
    [Table("Raza")]
    public class Raza
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("ID_ESPECIE")]
        public int IdEspecie { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }



    }
}
