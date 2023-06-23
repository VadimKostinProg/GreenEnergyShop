using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternetShop.UI.Areas.Customer.Controllers
{
    public class ArticlesController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult AboutUs()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AboutUs(string article)
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult PricingAndDelivery()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult PricingAndDelivery(string article)
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Contacts()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Contacts(string article)
        {
            return View();
        }
    }
}
