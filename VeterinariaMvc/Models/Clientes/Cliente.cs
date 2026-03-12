using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models.Clientes
{
    [Table("CLIENTE")]
    public class Cliente
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        public Usuario Usuario { get; set; }
    }
}