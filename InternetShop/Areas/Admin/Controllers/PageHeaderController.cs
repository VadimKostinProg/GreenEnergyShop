using InternetShop.Core.ServiceContracts;
using InternetShop.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;

namespace InternetShop.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PageHeaderController : Controller
    {
        private readonly ILogger<PageHeaderController> _logger;
        private readonly IPageHeaderService _pageHeaderService;

        public PageHeaderController(ILogger<PageHeaderController> logger, IPageHeaderService pageHeaderService)
        {
            _logger = logger;
            _pageHeaderService = pageHeaderService;
        }

        #region API CALLS
        public IActionResult Index()
        {
            try
            {
                List<string> images = _pageHeaderService.GetImagesOfHeader().ToList();

                return View(images);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("Error", new ErrorViewModel() { Description = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult AddImage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddImage(IFormFile? image)
        {
            try
            {
                _pageHeaderService.AddImageToHeader(image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("Error", new ErrorViewModel() { Description = ex.Message });
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(string url)
        {
            return View("Delete", url);
        }

        [HttpPost]
        public IActionResult PostDelete(string url)
        {
            bool result = _pageHeaderService.DeleteImageFormHeader(url);

            if (result)
            {
                return RedirectToAction("Index");
            }
            return View("Error", new ErrorViewModel() { Description = "Зображення не знайдено" });
        }
        #endregion
    }
}
