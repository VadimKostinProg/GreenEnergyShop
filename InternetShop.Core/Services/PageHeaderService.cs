using InternetShop.Core.ServiceContracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.Services
{
    public class PageHeaderService : IPageHeaderService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageService _imageService;

        public PageHeaderService(IWebHostEnvironment webHostEnvironment, IImageService imageService)
        {
            _webHostEnvironment = webHostEnvironment;
            _imageService = imageService;
        }

        public string AddImageToHeader(IFormFile image)
        {
            string url = _imageService.AddImage("header", image);

            string rootPath = _webHostEnvironment.WebRootPath;

            string productPath = rootPath + url;

            int width = 600, height = 200;

            using (Image addedImage = Image.Load(productPath))
            {
                addedImage.Mutate(image => image.Resize(width, height));

                addedImage.Save(productPath);
            }

            return url;
        }

        public bool DeleteImageFormHeader(string url)
        {
            return _imageService.DeleteImage(url);
        }

        public IEnumerable<string> GetImagesOfHeader()
        {
            return _imageService.GetImages("header");
        }
    }
}
