using InternetShop.Core.DTO;
using InternetShop.Core.ServiceContracts;
using InternetShop.UI.Areas.Admin.Controllers;
using InternetShop.UI.Areas.Customer.Models;
using InternetShop.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace InternetShop.UI.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IPageHeaderService _pageHeaderService;
        private readonly IArticleService _articleService;

        public HomeController(ILogger<HomeController> logger, ICategoryService categoryService, 
            IProductService productService, IOrderService orderService, IPageHeaderService pageHeaderService, 
            IArticleService articleService)
        {
            _logger = logger;
            _categoryService = categoryService;
            _productService = productService;
            _orderService = orderService;
            _pageHeaderService = pageHeaderService;
            _articleService = articleService;
        }

        #region API CALLS
        public async Task<IActionResult> Index()
        {
            await SetIndexTempData();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            await this.SetDefaultTempData();

            IEnumerable<ProductResponse> products = await _productService.GetAllProducts(convertPrice: true);

            if (string.IsNullOrEmpty(query))
            {
                ViewBag.Searched = false;
                return View();
            }

            query = query.ToLower();

            List<ProductResponse> searchedProducts = products
                .Where(product => product.Name.ToLower().Contains(query) || product.Category.Name.ToLower().Contains(query))
                .OrderBy(product => product.IsDiscountActive ? product.DiscountPrice : product.Price)
                .ToList();

            ViewBag.Searched = searchedProducts.Count > 0;

            return View(searchedProducts);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid productId)
        {
            ProductResponse? productResponse = await _productService.GetProductById(productId, convertPrice: true);

            if(productResponse == null)
            {
                return View("Error", new ErrorViewModel() { Description = "Обраного продукту не існує!"});
            }

            await this.SetDefaultTempData();

            return View(productResponse.ToProductViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Category(Guid categoryId)
        {
            CategoryResponse? categoryResponse = await _categoryService.GetCategoryById(categoryId);

            if(categoryResponse == null)
            {
                return View("Error");
            }

            await this.SetDefaultTempData();

            ViewBag.CategoryName = categoryResponse.Name;

            var products = await _productService.GetAllProducts(convertPrice: true);

            var productsOfCategory = products
                .Where(product => product.CategoryId == categoryId)
                .OrderBy(product => product.IsDiscountActive ? product.DiscountPrice : product.Price)
                .ToList();

            return View(productsOfCategory);
        }

        [HttpGet]
        public IActionResult ShoppingCart()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ProductInfo([FromQuery] Guid productId)
        {
            ProductResponse? productResponse = await _productService.GetProductById(productId, convertPrice: true);

            if(productResponse == null)
            {
                return NotFound("Id of product not found");
            }

            return Json(productResponse);
        }

        [HttpGet]
        public IActionResult OrderPage()
        {
            this.SetOrderTempData();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostOrder(OrderAddRequest orderAddRequest)
        {
            if(!ModelState.IsValid)
            {
                this.ServerSideValidationForConfirmOrder();
                this.SetOrderTempData();

                return View(orderAddRequest);
            }
            
            try
            {
                await _orderService.AddOrder(orderAddRequest);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Description = ex.Message });
            }

            return RedirectToAction(nameof(ConfirmedOrder));
        }

        [HttpGet]
        public IActionResult ConfirmedOrder()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

        #region SERVER SIDE VALIDATION
        private void ServerSideValidationForConfirmOrder()
        {
            List<string> errors = ModelState.Values
                    .SelectMany(value => value.Errors)
                    .Select(error => error.ErrorMessage)
                    .ToList();

            ViewData["Errors"] = errors;
        }
        #endregion

        #region TEMP DATA SET
        private async Task SetIndexTempData()
        {
            await this.SetDefaultTempData();

            IEnumerable<ProductResponse> products = await _productService.GetAllProducts(convertPrice: true);

            List<ProductResponse> popularProducts = products
                .Where(product => product.IsPopular)
                .OrderBy(product => product.IsDiscountActive ? product.DiscountPrice : product.Price)
                .ToList();

            List<ProductResponse> actionProducts = products
                .Where(product => product.IsDiscountActive)
                .OrderBy(product => product.IsDiscountActive ? product.DiscountPrice : product.Price)
                .ToList();

            if (popularProducts.Count == 0)
            {
                ViewBag.ShowPopular = false;
            }
            else
            {
                ViewBag.ShowPopular = true;
                ViewBag.PopularProducts = popularProducts;
            }

            if (actionProducts.Count == 0)
            {
                ViewBag.ShowAction = false;
            }
            else
            {
                ViewBag.ShowAction = true;
                ViewBag.ActionProducts = actionProducts;
            }

            List<string> images = _pageHeaderService.GetImagesOfHeader().ToList();
            ViewBag.IsHeaderPresent = images.Count > 0;
            ViewBag.ImagesUrl = images;

            
        }

        private async Task SetDefaultTempData()
        {
            var categories = await _categoryService.GetAllCategories();
            ViewBag.Categories = categories.ToList();

            var articles = await _articleService.GetAllArticles(includeHeaders: false);
            ViewBag.Articles = articles.ToList();
        }

        private void SetOrderTempData()
        {

            List<SelectListItem> convinientTime = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "Будь-який час", Value = "Будь-який час" },
                new SelectListItem(){ Text = "10:00 - 12:00", Value = "10:00 - 12:00" },
                new SelectListItem(){ Text = "12:00 - 14:00", Value = "12:00 - 14:00" },
                new SelectListItem(){ Text = "14:00 - 16:00", Value = "14:00 - 16:00" },
                new SelectListItem(){ Text = "16:00 - 18:00", Value = "16:00 - 18:00" },
                new SelectListItem(){ Text = "18:00 - 20:00", Value = "18:00 - 20:00" }
            };

            ViewData["TimeToCallList"] = convinientTime;
        }
        #endregion
    }
}