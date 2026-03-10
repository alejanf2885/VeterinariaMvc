namespace VeterinariaMvc.Dtos.Tratamiento
{
    public class SeguimientoDto
    {
        public int Id { get; set; }
        public int IdTratamiento { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Comentario { get; set; }
        public DateTime Fecha { get; set; }
    }
}
