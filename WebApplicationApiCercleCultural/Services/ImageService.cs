using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace WebApplicationApiCercleCultural.Services
{
    public class ImageService
    {
        private readonly string _imagesFolder;

        public ImageService()
        {
            // La carpeta se creará en ~/imagenes
            _imagesFolder = HostingEnvironment.MapPath("~/imagenes");

            if (!Directory.Exists(_imagesFolder))
            {
                Directory.CreateDirectory(_imagesFolder);
            }
        }

        public string SaveImage(HttpPostedFile file)
        {
            if (file == null || file.ContentLength == 0)
                throw new ArgumentException("Archivo no válido");

            // Validar extensión
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
                throw new ArgumentException("Formato no permitido");

            // Generar nombre único
            var fileName = Guid.NewGuid().ToString() + fileExtension;
            var fullPath = Path.Combine(_imagesFolder, fileName);

            file.SaveAs(fullPath);
            return fileName;
        }

        public string GetImagePath(string fileName)
        {
            return Path.Combine(_imagesFolder, fileName);
        }
    }
}