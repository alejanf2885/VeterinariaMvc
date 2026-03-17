using System.Collections.Generic;

namespace VeterinariaMvc.Areas.Veterinario.Models
{
    public class FichaConsultaCampoInput
    {
        public int IdCampo { get; set; }
        public string NombreCampo { get; set; } = string.Empty;
        public string? TipoDato { get; set; }
        public bool Obligatorio { get; set; }

        public string? ValorTexto { get; set; }
        public decimal? ValorNumero { get; set; }
        public System.DateTime? ValorFecha { get; set; }
        public bool? ValorBooleano { get; set; }
        public List<string> Opciones { get; set; } = new List<string>();
    }

    public class FichaConsultaSeccionInput
    {
        public string NombreSeccion { get; set; } = string.Empty;
        public List<FichaConsultaCampoInput> Campos { get; set; } = new List<FichaConsultaCampoInput>();
    }

    public class FichaConsultaViewModel
    {
        public int IdConsulta { get; set; }
        public int IdPlantilla { get; set; }
        public string NombrePlantilla { get; set; } = string.Empty;

        public List<FichaConsultaSeccionInput> Secciones { get; set; } = new List<FichaConsultaSeccionInput>();
    }
}
