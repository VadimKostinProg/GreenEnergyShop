using InternetShop.Core.DTO;
using InternetShop.Core.ServiceContracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string AddImage(string folder, IFormFile image)
        {
            if (image is null) throw new ArgumentNullException(nameof(image));

            string rootPath = _webHostEnvironment.WebRootPath;

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string productPath = Path.Combine(rootPath, @$"images\{folder}");

            if(!Directory.Exists(productPath))
                Directory.CreateDirectory(productPath);

            using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
            {
                image.CopyTo(fileStream);
            }

            string imageUrl = @$"\images\{folder}\" + fileName;

            return imageUrl;
        }

        public bool DeleteImage(string url)
        {
            string rootPath = _webHostEnvironment.WebRootPath;

            if (File.Exists(rootPath + "\\" + url))
            {
                File.Delete(rootPath + "\\" + url);
                return true;
            }
            else return false;
        }

        public IEnumerable<string> GetImages(string folder)
        {
            string rootPath = _webHostEnvironment.WebRootPath;

            string path = Path.Combine(rootPath, @$"images\{folder}");

            if (Directory.Exists(@$"images\{folder}"))
                throw new ArgumentException("Папка не знайдена");

            return Directory.GetFiles(path).Select(file => $@"\images\{folder}\{Path.GetFileName(file)}");
        }
    }
}
