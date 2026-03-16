using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models
{
    [Table("CLINICA")]
    public class Clinica
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("DIRECCION")]
        public string Direccion { get; set; }

        [Column("TELEFONO")]
        public string Telefono { get; set; }

        [Column("EMAIL")]
        public string Email { get; set; }

        [Column("IMAGEN")]
        public string? Imagen { get; set; }

        [Column("ESTADO")]
        public bool Estado { get; set; }

        [Column("ACTIVO")]
        public bool Activo { get; set; }

        [Column("CONFIGURADO")]
        public bool Configurado { get; set; }

    }
}
