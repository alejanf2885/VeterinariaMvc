using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models
{
    [Table("USUARIO")]
    public class Usuario
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("EMAIL")]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [MaxLength(20)]
        [Column("TELEFONO")]
        public string? Telefono { get; set; }

        [Column("ACTIVO")]
        public bool Activo { get; set; } = true;

        [Column("FECHA_CREACION")]
        public DateTime FechaCreacion { get; set; }

        [Column("ID_ROL")]
        public int IdRol { get; set; }

        [MaxLength(255)]
        [Column("IMAGEN")]
        public string? Imagen { get; set; }

    }
}