using InternetShop.Core.DTO;
using InternetShop.Core.ServiceContracts;
using InternetShop.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternetShop.UI.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("AboutUs")]
        public async Task<IActionResult> AboutUs()
        {
            var article = await _articleService.GetArticleByTitle("Про нас");

            if(article == null)
                return View();

            return View("Article", article.ToArticleUpdateRequest());
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("PricingAndDelivery")]
        public async Task<IActionResult> PricingAndDelivery()
        {
            var article = await _articleService.GetArticleByTitle("Доставка та оплата");

            if (article == null)
                return View();

            return View("Article", article.ToArticleUpdateRequest());
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Contacts")]
        public async Task<IActionResult> Contacts()
        {
            var article = await _articleService.GetArticleByTitle("Контакти");

            if (article == null)
                return View();

            return View("Article", article.ToArticleUpdateRequest());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid articleId)
        {
            var article = await _articleService.GetArticleById(articleId);

            if (article == null)
                return View("Error", new ErrorViewModel() { Description = "Стаття не знайдена" });

            return View(article.ToArticleUpdateRequest());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostArticle(ArticleUpdateRequest articleUpdateRequest)
        {
            var articleUpdated = await _articleService.UpdateArticle(articleUpdateRequest);
            if (articleUpdated == null)
                return View();

            return View("Article", articleUpdated.ToArticleUpdateRequest());
        }
    }
}
