using VeterinariaMvc.Enums;

namespace VeterinariaMvc.Services.Imagenes
{
    public interface IImagenService
    {
        Task<string> SubirImagenAsync(IFormFile archivo, CarpetaDestino carpeta, int tamano = 500);
        Task BorrarImagenAsync(string rutaRelativa);
    }
}
