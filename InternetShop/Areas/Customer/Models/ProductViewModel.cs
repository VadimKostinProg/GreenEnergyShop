using InternetShop.Core.Domain.Entities;
using InternetShop.Core.DTO;
using InternetShop.Core.Enums;

namespace InternetShop.UI.Areas.Customer.Models
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public decimal Price { get; set; }
        public bool IsDiscountActive { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int Quantity { get; set; }
        public ProductStatus Status { get; set; }
        public string? ImageUrl { get; set; }

        public Dictionary<string, string> Characteristics { get; set; } = new Dictionary<string, string>();
    }

    public static class ProductResponseExt
    {
        public static ProductViewModel ToProductViewModel(this ProductResponse productResponse)
        {
            ProductViewModel productViewModel = new ProductViewModel()
            {
                Id = productResponse.Id,
                Name = productResponse.Name,
                Description = productResponse.Description,
                CategoryId = productResponse.CategoryId,
                CategoryName = productResponse.Category?.Name,
                Price = productResponse.Price,
                IsDiscountActive = productResponse.IsDiscountActive,
                DiscountPrice = productResponse.DiscountPrice,
                Quantity = 1,
                Status = productResponse.Status,
                ImageUrl = productResponse.ImageUrl,
                Characteristics = productResponse.Characteristics
            };

            return productViewModel;
        }
    }
}
