using VeterinariaMvc.Dtos.Session;
using VeterinariaMvc.Models;

namespace VeterinariaMvc.Mappers
{
    public static class UsuarioMapper
    {

        public static UsuarioSessionDto ToSessionDto(this Usuario usuario)
        {
            if(usuario == null)
            {
                return null;
            }

            UsuarioSessionDto usuarioSessionDto = new UsuarioSessionDto
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Imagen = usuario.Imagen, 
                IdRol = usuario.IdRol
            };

            return usuarioSessionDto;
        }
    }
}
