using InternetShop.Core.DTO;
using InternetShop.Core.ServiceContracts;
using InternetShop.Core.Services;
using InternetShop.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace InternetShop.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoriesController(ILogger<CategoriesController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<CategoryResponse> cateogries = await _categoryService.GetAllCategories();

            return View(cateogries);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryAddRequest categoryAddRequest)
        {
            if(!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(error => error.ErrorMessage)
                    .ToList();

                ViewData["Errors"] = errors;

                return View(categoryAddRequest);
            }

            try
            {
                CategoryResponse categoryResponse = await _categoryService.AddCategory(categoryAddRequest);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("Error", new ErrorViewModel() { RequestId = "/Admin/Categories/Edit", Description = ex.Message });
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid categoryId)
        {
            _logger.LogInformation("{ControllerName}.{ActionName} executing.", nameof(CategoriesController), nameof(Edit));

            CategoryResponse? categoryResponse = await _categoryService.GetCategoryById(categoryId);

            if(categoryResponse == null)
            {
                return View("Error", new ErrorViewModel() { RequestId = "/Admin/Categories/Edit" });
            }

            CategoryUpdateRequest categoryUpdateRequest = categoryResponse.ToCateogryUpdateRequest();

            return View(categoryUpdateRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryUpdateRequest categoryUpdateRequest)
        {
            if (!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(error => error.ErrorMessage)
                    .ToList();

                ViewData["Errors"] = errors;

                return View(categoryUpdateRequest);
            }

            CategoryResponse? categoryResponse;

            try
            {
                categoryResponse = await _categoryService.UpdateCategory(categoryUpdateRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("Error", new ErrorViewModel() { RequestId = "/Admin/Categories/Edit", Description = ex.Message });
            }

            if (categoryResponse == null)
            {
                return View("Error", new ErrorViewModel() { RequestId = "/Admin/Categories/Edit" });
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid categoryId)
        {
            _logger.LogInformation("{ControllerName}.{ActionName} executing.", nameof(CategoriesController), nameof(Delete));

            CategoryResponse? categoryResponse = await _categoryService.GetCategoryById(categoryId);

            if (categoryResponse == null)
            {
                return View("Error", new ErrorViewModel() { RequestId = "/Admin/Categories/Edit", Description = "Категорія не знайдена" });
            }

            return View(categoryResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CategoryResponse categoryToDelete)
        {
            bool result = await _categoryService.DeleteCategory(categoryToDelete.Id);

            if(result == false)
            {
                return View("Error", new ErrorViewModel() { RequestId = "/Admin/Categories/Delete", Description = "Категорія не знайдена" });
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Characteristics(Guid categoryId)
        {
            CategoryResponse? categoryResponse = await _categoryService.GetCategoryById(categoryId);

            if(categoryResponse == null)
            {
                return NotFound("Категорія не знайдена");
            }

            return Json(categoryResponse.CharacteristicsList);
        }
        #endregion
    }
}
