using System.Collections.Generic;

namespace VeterinariaMvc.Areas.Veterinario.Models
{
	public class PlantillaDetalleViewModel
	{
		public int Id { get; set; }
		public string Nombre { get; set; } = string.Empty;
		public bool Activa { get; set; }

		public List<PlantillaSeccionDetalle> Secciones { get; set; } = new List<PlantillaSeccionDetalle>();
	}

	public class PlantillaSeccionDetalle
	{
		public int Id { get; set; }
		public string Nombre { get; set; } = string.Empty;
		public int? Orden { get; set; }
		public List<PlantillaCampoDetalle> Campos { get; set; } = new List<PlantillaCampoDetalle>();
	}

	public class PlantillaCampoDetalle
	{
		public int Id { get; set; }
		public string Nombre { get; set; } = string.Empty;
		public string? TipoDato { get; set; }
		public bool? Obligatorio { get; set; }
		public int? Orden { get; set; }
		public List<string> Opciones { get; set; } = new List<string>();
	}
}
