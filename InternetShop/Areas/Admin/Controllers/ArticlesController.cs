using InternetShop.Core.DTO;
using InternetShop.Core.ServiceContracts;
using InternetShop.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternetShop.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetAllArticles(includeHeaders: false);

            return View(articles);
        }

        [HttpGet]
        public async Task<IActionResult> AddArticle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddArticles(ArticleAddRequest articleAddRequest)
        {
            try
            {
                await _articleService.AddArticle(articleAddRequest);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { Description = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromQuery] Guid articleId)
        {
            var article = await _articleService.GetArticleById(articleId);

            if(article == null)
            {
                return View("Error", new ErrorViewModel() { Description = "Стаття не знайдена." });
            }

            return View(article.ToArticleUpdateRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ArticleUpdateRequest articleUpdateRequest)
        {
            try
            {
                await _articleService.UpdateArticle(articleUpdateRequest);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { Description = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeletePage([FromQuery] Guid articleId)
        {
            var article = await _articleService.GetArticleById(articleId);

            if (article == null)
            {
                return View("Error", new ErrorViewModel() { Description = "Стаття не знайдена." });
            }

            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid articleId)
        {
            try
            {
                await _articleService.DeleteArticle(articleId);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View("Error", new ErrorViewModel() { Description = ex.Message });
            }
        }
    }
}
