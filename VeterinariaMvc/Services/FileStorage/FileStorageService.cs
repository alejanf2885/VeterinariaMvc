using VeterinariaMvc.Enums;

namespace VeterinariaMvc.Services.FileStorage
{
    public class FileStorageService : IFileStorageService
    {

        private IWebHostEnvironment _env;

        public FileStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public Task BorrarArchivoAsync(string rutaRelativa)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GuardarArchivoAsync(Stream contenido, string nombreArchivo, CarpetaDestino carpeta)
        {

            string nombreCarpeta = carpeta.ToString().ToLower();

            string rutaCarpeta = Path.Combine(_env.WebRootPath, "images", nombreCarpeta);
            if (!Directory.Exists(rutaCarpeta))
            {
                Directory.CreateDirectory(rutaCarpeta);
            }

            string rutaFisica = Path.Combine(rutaCarpeta, nombreArchivo);

            using (var fileStream = new FileStream(rutaFisica, FileMode.Create))
            {
                await contenido.CopyToAsync(fileStream);
            }

            return $"/images/{nombreCarpeta}/{nombreArchivo}";
        }
    }
}
