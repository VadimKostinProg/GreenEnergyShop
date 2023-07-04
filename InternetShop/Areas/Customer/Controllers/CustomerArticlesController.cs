using InternetShop.Core.Domain.Entities;
using InternetShop.Core.DTO;
using InternetShop.Core.ServiceContracts;
using InternetShop.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternetShop.UI.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CustomerArticlesController : Controller
    {
        private readonly IArticleService _articleService;

        public CustomerArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        #region HEADER ARTICLES
        [AllowAnonymous]
        [HttpGet]
        [Route("AboutUs")]
        public async Task<IActionResult> AboutUs()
        {
            try
            {
                var article = await _articleService.GetArticleByTitle("Про нас");

                if (article == null)
                    return View();

                return View("Article", article);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { Description = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("PricingAndDelivery")]
        public async Task<IActionResult> PricingAndDelivery()
        {
            try
            {
                var article = await _articleService.GetArticleByTitle("Доставка та оплата");

                if (article == null)
                    return View();

                return View("Article", article);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { Description = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Contacts")]
        public async Task<IActionResult> Contacts()
        {
            try
            {
                var article = await _articleService.GetArticleByTitle("Контакти");

                if (article == null)
                    return View();

                return View("Article", article);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { Description = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid articleId)
        {
            try
            {
                var article = await _articleService.GetArticleById(articleId);

                if (article == null)
                    return View("Error", new ErrorViewModel() { Description = "Стаття не знайдена" });

                return View(article.ToArticleUpdateRequest());
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { Description = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostArticle(ArticleUpdateRequest articleUpdateRequest)
        {
            try
            {
                articleUpdateRequest.IsHeaderArticle = true;
                var articleUpdated = await _articleService.UpdateArticle(articleUpdateRequest);

                if (articleUpdated == null)
                    return View();

                return View("Article", articleUpdated);
            }
            catch(Exception ex)
            {
                return View("Error", new ErrorViewModel() { Description = ex.Message });
            }
        }
        #endregion

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Article([FromQuery] Guid articleId)
        {
            try
            {
                var article = await _articleService.GetArticleById(articleId);

                return View("Article", article);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { Description = ex.Message });
            }
        }
    }
}
