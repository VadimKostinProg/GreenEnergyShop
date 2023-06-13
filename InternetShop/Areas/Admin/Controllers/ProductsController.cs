using InternetShop.Core.DTO;
using InternetShop.Core.Enums;
using InternetShop.Core.ServiceContracts;
using InternetShop.Core.ValidationAtributes;
using InternetShop.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InternetShop.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IImageService _imageService;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService,
        ICategoryService categoryService, IImageService imageService)
        {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
            _imageService = imageService;
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<ProductResponse> products = await _productService.GetAllProducts();

                return View(products);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("Error", new ErrorViewModel() { Description = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                await SetTempDataForCreate();

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("Error", new ErrorViewModel() { Description = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductAddRequest productAddRequest, IFormFile? image)
        {
            await SetTempDataForCreate();

            if (!ModelState.IsValid)
            {
                ServerSideValidationForCreate();

                return View(productAddRequest);
            }

            try
            {
                if (image != null)
                {
                    productAddRequest.ImageUrl = _imageService.AddImage("products", image);

                    ProductResponse productResponse = await _productService.AddProduct(productAddRequest);

                    return RedirectToAction("Index");
                }
                else
                {
                    throw new ArgumentNullException("Зображення є обов'язковим");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                ViewData["Errors"] = new List<string>() { ex.Message };

                return View(productAddRequest);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid productId)
        {
            try
            {
                var productResponse = await SetTempDataForEdit(productId);

                if (productResponse is null)
                    return View("Error", new ErrorViewModel());

                ProductUpdateRequest productUpdateRequest = productResponse.ToProductUpdateRequest();

                return View(productUpdateRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("Error", new ErrorViewModel() { Description = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductUpdateRequest productUpdateRequest, IFormFile? image)
        {
            var productResponse = await SetTempDataForEdit(productUpdateRequest.Id);
            if (productResponse is null)
                return View("Error", new ErrorViewModel() { Description = "Продукту із вказаним ідентифікатором не існує." });

            if (!ModelState.IsValid)
            {
                ServerSideValidationForEdit();

                return View(productUpdateRequest);
            }

            try
            {
                if (image != null)
                {
                    string oldFilePath = productResponse.ImageUrl;

                    bool result = _imageService.DeleteImage(oldFilePath);

                    productUpdateRequest.ImageUrl = _imageService.AddImage("products", image);
                }

                await _productService.UpdateProduct(productUpdateRequest);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                ViewData["Errors"] = new List<string>() { ex.Message };

                return View(productUpdateRequest);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid productId)
        {
            ProductResponse? productResponse = await _productService.GetProductById(productId);

            if (productResponse == null)
            {
                return View("Error", new ErrorViewModel() { Description = "Продукт не знайдений"});
            }

            return View(productResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductResponse productResponse)
        {
            string oldFilePath = productResponse.ImageUrl;

            _imageService.DeleteImage(oldFilePath);

            bool result = await _productService.DeleteProduct(productResponse.Id);

            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Description = "Продукт не знайдений" });
            }
        }
        #endregion

        #region SERVER SIDE VALIDATION
        private void ServerSideValidationForCreate()
        {
            IEnumerable<string> errors = ModelState.Values.SelectMany(x => x.Errors)
                    .Select(error => error.ErrorMessage);

            ViewData["Errors"] = errors;
        }

        private void ServerSideValidationForEdit()
        {
            List<string> errors = ModelState.Values
                    .SelectMany(value => value.Errors)
                    .Select(error => error.ErrorMessage)
                    .ToList();

            var validationAtribute = new RequiredWhenActive();

            if (errors.Contains(validationAtribute.ErrorMessageWhenActive))
                errors.Remove(validationAtribute.ErrorMessageWhenActive);
            if (errors.Contains(validationAtribute.ErrorMessageWhenDiscountIsBigger))
                errors.Remove(validationAtribute.ErrorMessageWhenDiscountIsBigger);

            ViewData["Errors"] = errors;
        }
        #endregion

        #region TEMP DATA SET
        private async Task SetTempDataForCreate()
        {
            IEnumerable<SelectListItem> categoryList = (await _categoryService.GetAllCategories())
                .Select(category => new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });

            IEnumerable<SelectListItem> currencyList = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "USD", Value = "USD" },
                new SelectListItem() { Text = "EUR", Value = "EUR" },
            };

            ViewData["CategoryList"] = categoryList;
            ViewData["CurrencyList"] = currencyList;
        }

        private async Task<ProductResponse?> SetTempDataForEdit(Guid productId)
        {
            ProductResponse? productResponse = await _productService.GetProductById(productId);

            if (productResponse == null)
            {
                return null;
            }

            IEnumerable<SelectListItem> categoryList = (await _categoryService.GetAllCategories())
                .Select(category => new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });

            IEnumerable<SelectListItem> currencyList = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "USD", Value = "USD" },
                new SelectListItem() { Text = "EUR", Value = "EUR" },
            };

            IEnumerable<SelectListItem> statusList = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Є в наявності", Value = ProductStatus.InStock.ToString() },
                new SelectListItem() { Text = "Закінчується", Value = ProductStatus.Ends.ToString() },
                new SelectListItem() { Text = "Немає в наявності", Value = ProductStatus.NotAvialable.ToString() },
            };

            ViewData["CategoryList"] = categoryList;
            ViewData["CurrencyList"] = currencyList;
            ViewData["StatusList"] = statusList;

            CategoryResponse? categoryResponse = await _categoryService.GetCategoryById(productResponse.CategoryId);
            if (categoryResponse != null)
            {
                ViewData["CharacteristicsName"] = categoryResponse.CharacteristicsList;
            }

            return productResponse;
        }
        #endregion
    }
}
