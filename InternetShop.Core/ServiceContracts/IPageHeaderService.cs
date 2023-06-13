using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace InternetShop.Core.ServiceContracts
{
    /// <summary>
    /// Tools for managing header of main page.
    /// </summary>
    public interface IPageHeaderService
    {
        /// <summary>
        /// Method for adding new image to main page header.
        /// </summary>
        /// <param name="formFile">Image to add.</param>
        /// <returns>Url of added image.</returns>
        string AddImageToHeader(IFormFile image);

        /// <summary>
        /// Method for getting all images of main page header.
        /// </summary>
        /// <returns>Collection of urls of all images.</returns>
        IEnumerable<string> GetImagesOfHeader();

        /// <summary>
        /// Method for deleting image from main page header.
        /// </summary>
        /// <param name="url">Url of image to delete.</param>
        /// <returns>True - if deletion was successfull, otherwise - false.</returns>
        bool DeleteImageFormHeader(string url);
    }
}
