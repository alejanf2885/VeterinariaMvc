using VeterinariaMvc.Enums;

namespace VeterinariaMvc.Services.FileStorage
{
    public interface IFileStorageService
    {
        Task<string> GuardarArchivoAsync(Stream contenido, string nombreArchivo, CarpetaDestino carpeta);
        Task BorrarArchivoAsync(string rutaRelativa);
    }
}
