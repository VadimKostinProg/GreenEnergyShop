using InternetShop.Core.Domain.Entities;
using InternetShop.Core.Domain.RepositoryContracts;
using InternetShop.Core.DTO;
using InternetShop.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderHeaderRepository _orderHeaderRepository;
        private readonly IOrderDetailsRepository _orderDetailsRepository;

        private readonly IProductService _productService;

        private readonly IEmailSender _emailSender;

        private readonly UserManager<IdentityUser> _userManager;

        public OrderService(IOrderHeaderRepository orderHeaderRepository, IOrderDetailsRepository orderDetailsRepository, 
            IProductService productService, IEmailSender emailSender, UserManager<IdentityUser> userManager)
        {
            _orderHeaderRepository = orderHeaderRepository;
            _orderDetailsRepository = orderDetailsRepository;
            _productService = productService;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public async Task<OrderResponse> AddOrder(OrderAddRequest orderAddRequest)
        {
            OrderHeader orderHeader = new()
            {
                Id = Guid.NewGuid(),
                CustomerPhoneNumber = orderAddRequest.CustomerPhoneNumber,
                TimeToCall = orderAddRequest.TimeToCall,
                OrderConfirmationTime = DateTime.Now
            };

            List<OrderDetails> orderDetailsList = new List<OrderDetails>();

            for(int i = 0; i < orderAddRequest.ProductsId.Count; i++)
            {
                ProductResponse? product = await _productService.GetProductById(orderAddRequest.ProductsId[i], convertPrice: true);

                if (product is null)
                    throw new KeyNotFoundException("Продукту із замовлення більше не існує");

                OrderDetails orderDetails = new()
                {
                    Id = Guid.NewGuid(),
                    OrderHeaderId = orderHeader.Id,
                    ProductId = orderAddRequest.ProductsId[i],
                    Count = orderAddRequest.Quantities[i],
                    Price = product.IsDiscountActive ? product.DiscountPrice.Value : product.Price
                };

                orderDetailsList.Add(orderDetails);
            }

            _orderHeaderRepository.Create(orderHeader);

            await _orderHeaderRepository.Save();

            foreach(var orderDetails in orderDetailsList)
            {
                _orderDetailsRepository.Create(orderDetails);
            }

            await _orderDetailsRepository.Save();

            var orderHeaderCreated = _orderHeaderRepository.GetValueById(orderHeader.Id);
            var orderDetailsCreatedList = _orderDetailsRepository.GetByOrderHeaderId(orderHeader.Id).ToList();

            OrderResponse response = ConvertToOrderResponse(orderHeaderCreated, orderDetailsCreatedList);

            //Sending notification to admins
            var admins = await _userManager.GetUsersInRoleAsync("Admin");

            string message = new StringBuilder()
                .Append("<h1>Нове замовлення</h1>")
                .Append($"<p>Користувач із номером {response.CustomerPhoneNumber} зробив замовлення.</p>")
                .Append($"<p>Перейдіть на сторінку обробки замовлень для перегляду деталей.</p>")
                .ToString();

            foreach (var admin in admins)
            {
                await _emailSender.SendEmailAsync(admin.Email, "Нове замовлення", message);
            }

            return response;
        }

        public async Task<bool> ConfirmOrder(Guid orderId)
        {
            OrderHeader? orderHeader = _orderHeaderRepository.GetValueById(orderId);

            if(orderHeader is null)
                return false;

            orderHeader.Confirmed = true;

            await _orderHeaderRepository.Save();

            return true;
        }

        public async Task<bool> DeleteOrder(Guid orderId)
        {
            bool result = _orderHeaderRepository.Delete(orderId);

            if (result)
            {
                await _orderHeaderRepository.Save();
            }

            return result;
        }

        public async Task DeleteRangeForDates(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("Дата початку інтервалу менша дату кіня інтервалу");

            var orders = _orderHeaderRepository.GetAll()
                .Where(order => order.OrderConfirmationTime.Date >= startDate.Date && order.OrderConfirmationTime.Date <= endDate.Date)
                .ToList();

            foreach(var order in orders)
            {
                _orderHeaderRepository.Delete(order.Id);
            }

            await _orderHeaderRepository.Save();
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrders()
        {
            var orderHeaders = _orderHeaderRepository.GetAll();

            List<OrderResponse> orders = new List<OrderResponse>();

            foreach(var orderHeader in orderHeaders)
            {
                IEnumerable<OrderDetails> orderDetailsList = _orderDetailsRepository.GetByOrderHeaderId(orderHeader.Id);
                OrderResponse orderResponse = ConvertToOrderResponse(orderHeader, orderDetailsList);
                orders.Add(orderResponse);
            }

            return orders;
        }

        public async Task<IEnumerable<OrderDetailsResponse>> GetDetailsByOrderId(Guid orderId)
        {
            return _orderDetailsRepository.GetByOrderHeaderId(orderId)
                .Select(orderDetails => orderDetails.ToOrderDetailsResponse())
                .ToList();
        }

        public async Task<OrderResponse> GetOrderById(Guid orderId)
        {
            var orderHeader = _orderHeaderRepository.GetValueById(orderId);

            if(orderHeader is null)
                throw new KeyNotFoundException("Замовлення не знайдене");

            var details = _orderDetailsRepository.GetByOrderHeaderId(orderId).ToList();

            Dictionary<string, int> detailsDict = new Dictionary<string, int>();

            OrderResponse response = ConvertToOrderResponse(orderHeader, details);

            return response;
        }

        private OrderResponse ConvertToOrderResponse(OrderHeader orderHeader, IEnumerable<OrderDetails> orderDetailsList)
        {
            Dictionary<string, int> detailsDict = new Dictionary<string, int>();

            decimal totalCost = 0;

            foreach (var detail in orderDetailsList)
            {
                detailsDict.Add(detail.Product.Name, detail.Count);
                totalCost += detail.Price * detail.Count;
            }

            OrderResponse response = new()
            {
                Id = orderHeader.Id,
                CustomerPhoneNumber = orderHeader.CustomerPhoneNumber,
                TimeToCall = orderHeader.TimeToCall,
                OrderConfirmationTime = orderHeader.OrderConfirmationTime,
                Details = detailsDict,
                TotalCost = totalCost,
                Confirmed = orderHeader.Confirmed
            };

            return response;
        }
    }
}
