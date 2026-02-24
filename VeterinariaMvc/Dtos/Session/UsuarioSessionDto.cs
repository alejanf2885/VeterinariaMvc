namespace VeterinariaMvc.Dtos.Session
{
    public class UsuarioSessionDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string? Imagen { get; set; }
        public int IdRol { get; set; }
    }
}
