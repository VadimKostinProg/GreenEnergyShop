using InternetShop.Core.DTO;
using InternetShop.Core.ServiceContracts;
using InternetShop.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternetShop.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderService _orderService;

        public OrdersController(ILogger<OrdersController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var orders = (await _orderService.GetAllOrders())
                .OrderByDescending(order => order.OrderConfirmationTime)
                .ToList();

                return View(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("Error", new ErrorViewModel { Description = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid orderId)
        {
            try
            {
                var orderDetails = await _orderService.GetDetailsByOrderId(orderId);

                return View(orderDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("Error", new ErrorViewModel { Description = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Confirm([FromQuery] Guid orderId)
        {
            try
            {
                bool result = await _orderService.ConfirmOrder(orderId);

                if (result)
                    return Ok("Successfully confirmed");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("Error", new ErrorViewModel { Description = ex.Message });
            }

            return NotFound("Id of order not found");
        }

        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRange(DateTime start, DateTime end)
        {
            try
            {
                await _orderService.DeleteRangeForDates(start, end);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("Error", new ErrorViewModel { Description = ex.Message });
            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}
