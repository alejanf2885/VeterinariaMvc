namespace VeterinariaMvc.Dtos.Mascota
{
    public class MascotaResumenDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Especie { get; set; } 
        public string Raza { get; set; }  
        public string? Imagen { get; set; }
        public string Sexo { get; set; }

    }
}
