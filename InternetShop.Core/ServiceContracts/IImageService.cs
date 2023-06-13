using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.ServiceContracts
{
    /// <summary>
    /// Tools for managing images
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Method for adding new image to application root folder.
        /// </summary>
        /// <param name="folder">Name of folder to add image.</param>
        /// <param name="image">Image to add.</param>
        /// <returns>Url of added image.</returns>
        string AddImage(string folder, IFormFile image);

        /// <summary>
        /// Method for getting url of all images in the folder.
        /// </summary>
        /// <param name="folder">Folder to read all images inside.</param>
        /// <returns>Collection IEnumerable of url of images.</returns>
        IEnumerable<string> GetImages(string folder);

        /// <summary>
        /// Method for deleting image from application root folder.
        /// </summary>
        /// <param name="url">Url of image to delete.</param>
        /// <returns>True - if deleting was successful, otherwise - false.</returns>
        bool DeleteImage(string url);
    }
}
