using InternetShop.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.ServiceContracts
{
    /// <summary>
    /// Business logic for order managment.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Method for adding new order to the data store.
        /// </summary>
        /// <param name="orderAddRequest">Order to add.</param>
        /// <returns>Added order.</returns>
        Task<OrderResponse> AddOrder(OrderAddRequest orderAddRequest);

        /// <summary>
        /// Method for reading order from the data store by it`s guid.
        /// </summary>
        /// <param name="orderId">Guid of order to read.</param>
        /// <returns>Order with passed guid, null - if order does not exist.</returns>
        Task<OrderResponse> GetOrderById(Guid orderId);

        /// <summary>
        /// Method for reading all details of entered order.
        /// </summary>
        /// <param name="orderId">Guid of order to read details.</param>
        /// <returns>Collection IEnumerable of OrderDetailsResponse.</returns>
        Task<IEnumerable<OrderDetailsResponse>> GetDetailsByOrderId(Guid orderId);

        /// <summary>
        /// Method for reading all orders from the data store in order of their confirmation.
        /// </summary>
        /// <returns>Collection IEnumerable of OrderResponse.</returns>
        Task<IEnumerable<OrderResponse>> GetAllOrders();

        /// <summary>
        /// Mehtod for confirming order.
        /// </summary>
        /// <param name="orderId">Guid of order to confirm.</param>
        Task<bool> ConfirmOrder(Guid orderId);

        /// <summary>
        /// Method for deleting order from the data store.
        /// </summary>
        /// <param name="orderId">Guid of order to delete.</param>
        /// <returns>True - if deleting is successful, otherwise - false.</returns>
        Task<bool> DeleteOrder(Guid orderId);

        /// <summary>
        /// Method for deleting range of orders between passed dates.
        /// </summary>
        /// <param name="startDate">Start date of range.</param>
        /// <param name="endDate">End date of range.</param>
        Task DeleteRangeForDates(DateTime startDate, DateTime endDate);
    }
}
