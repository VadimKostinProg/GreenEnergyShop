using InternetShop.Core.ServiceContracts;
using InternetShop.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternetShop.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CurrencyController : Controller
    {
        private readonly ILogger<CurrencyController> _logger;
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ILogger<CurrencyController> logger, ICurrencyService currencyService)
        {
            _logger = logger;
            _currencyService = currencyService;
        }

        #region API CALLS
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                SetTempDataForIndex();

                return View();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("Error", new ErrorViewModel() { Description = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePrice(decimal USDPrice, decimal EURPrice)
        {
            try
            {
                await _currencyService.SetPriceInUah(USDPrice, Core.Enums.CurrencyName.USD);
                await _currencyService.SetPriceInUah(EURPrice, Core.Enums.CurrencyName.EUR);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("Error", new ErrorViewModel() { Description = ex.Message });
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region TEMP DATA
        private void SetTempDataForIndex()
        {
            decimal USDPrice, EURPrice;
            USDPrice = EURPrice = 0;

            try
            {
                USDPrice = _currencyService.GetPriceInUAH(Core.Enums.CurrencyName.USD);
                EURPrice = _currencyService.GetPriceInUAH(Core.Enums.CurrencyName.EUR);
            }
            catch (Exception) { }

            ViewBag.USDPrice = USDPrice;
            ViewBag.EURPrice = EURPrice;
        }
        #endregion
    }
}
