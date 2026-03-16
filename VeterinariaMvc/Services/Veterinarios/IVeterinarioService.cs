namespace VeterinariaMvc.Services.Veterinarios
{
    public interface IVeterinarioService
    {
        Task<bool> RegistrarVeterinarioAsync(
         int idUsuario,
         int idClinica,
         string? numeroColegiado);


        Task<bool> EliminarVeterinarioAsync(int idUsuario, int idClinica);
    }
}
