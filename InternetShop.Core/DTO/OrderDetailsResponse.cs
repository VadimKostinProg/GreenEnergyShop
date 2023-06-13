using InternetShop.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.DTO
{
    public class OrderDetailsResponse
    {
        public Guid Id { get; set; }
        public Guid OrderHeaderId { get; set; }
        public string ProductName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Cost
        {
            get
            {
                return Quantity * Price;
            }
        }
    }

    public static class OrderDetailsExt
    {
        public static OrderDetailsResponse ToOrderDetailsResponse(this OrderDetails orderDetails)
        {
            return new OrderDetailsResponse
            {
                Id = orderDetails.Id,
                OrderHeaderId = orderDetails.OrderHeaderId,
                ProductName = orderDetails.Product.Name,
                CategoryName = orderDetails.Product.Category.Name,
                Quantity = orderDetails.Count,
                Price = orderDetails.Price
            };
        }
    }
}
