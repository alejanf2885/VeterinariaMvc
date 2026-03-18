using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaMvc.Models.Plantillas
{
    [Table("FICHA_VALOR")]
    public class FichaValor
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("ID_FICHA")]
        public int IdFicha { get; set; }

        [Column("ID_CAMPO")]
        public int IdCampo { get; set; }

        [Column("VALOR_TEXTO")]
        public string? ValorTexto { get; set; }

        [Column("VALOR_NUMERO")]
        public decimal? ValorNumero { get; set; }

        [Column("VALOR_FECHA")]
        public DateTime? ValorFecha { get; set; }

        [Column("VALOR_BOOLEANO")]
        public bool? ValorBooleano { get; set; }
    }
}
