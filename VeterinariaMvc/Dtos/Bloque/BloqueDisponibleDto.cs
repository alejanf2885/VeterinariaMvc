using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Dtos.Bloque
{
    [Keyless]
    [Table("VW_BLOQUES_DISPONIBLES")] 
    public class BloqueDisponibleDto
    {
        [Column("IdBloque")]
        public int IdBloque { get; set; }

        [Column("IdClinica")]
        public int IdClinica { get; set; }

        [Column("Fecha")]
        public DateTime Fecha { get; set; }

        [Column("Turno")]
        public string Turno { get; set; }

        [Column("CapacidadTotal")]
        public int CapacidadTotal { get; set; }

        [Column("OcupacionActual")]
        public int OcupacionActual { get; set; }

        [Column("HuecosLibres")]
        public int HuecosLibres { get; set; }
    }
}