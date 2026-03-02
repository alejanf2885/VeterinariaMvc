using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models
{
    [Table("MASCOTA")]
    public class Mascota
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("ID_CLIENTE")]
        public int IdCliente { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("SEXO")]
        public string? Sexo { get; set; }

        [Column("FECHA_NACIMIENTO")]
        public DateTime? FechaNacimiento { get; set; }

        [Column("PESO_ACTUAL")]
        public double? PesoActual { get; set; }

        [Column("IMAGEN")]
        public string? Imagen { get; set; }

        [Column("FALLECIDO")]
        public bool? Fallecido { get; set; }

        [Column("ID_ESPECIE")]
        public int? IdEspecie { get; set; }

        [Column("ID_RAZA")]
        public int? IdRaza { get; set; }
    }
}