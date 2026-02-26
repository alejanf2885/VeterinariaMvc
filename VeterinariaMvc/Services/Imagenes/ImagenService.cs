using VeterinariaMvc.Enums;
using VeterinariaMvc.Services.Archivos;
using VeterinariaMvc.Services.FileStorage;
using VeterinariaMvc.Services.SaneadorFotos;

namespace VeterinariaMvc.Services.Imagenes
{
    public class ImagenService : IImagenService
    {
        private INombreArchivoService _nombreService;
        private IProcesadorImagenService _procesador;
        private IFileStorageService _storage;

        public ImagenService
            (INombreArchivoService nombreService,
            IProcesadorImagenService procesador,
            IFileStorageService storage)
        {
            this._nombreService = nombreService;
            this._procesador = procesador;
            this._storage = storage;
        }

        public async Task<string> SubirImagenAsync(IFormFile archivo, CarpetaDestino carpeta, int tamano = 300)
        {

            // 1 Generar nombre unico 
            string nombreUnico = this._nombreService.GenerarNombreUnico(archivo.FileName, ".webp");

            // 2 Procesamiento de la foto

            using (var streamLimpio = await this._procesador.SanearYProcesarAsync(archivo, tamano))
            {
                // 3 Guardar
                return await _storage.GuardarArchivoAsync(streamLimpio, nombreUnico, carpeta);
            }



        }
    }
}
