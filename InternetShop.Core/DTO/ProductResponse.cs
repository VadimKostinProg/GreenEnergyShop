using InternetShop.Core.Domain.Entities;
using InternetShop.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.DTO
{
    public class ProductResponse
    {
        public Guid Id { get; set; }   
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        public string Currency { get; set; } = null!;
        public decimal Price { get; set; }
        public bool IsDiscountActive { get; set; }
        public decimal? DiscountPrice { get; set; }
        public ProductStatus Status { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsPopular { get; set; }

        public Dictionary<string, string> Characteristics { get; set; } = new Dictionary<string, string>();

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if(obj.GetType() != typeof(ProductResponse)) 
                return false;

            ProductResponse other = (ProductResponse)obj;

            return this.Id == other.Id && this.Name == other.Name && this.Description == other.Description &&
                this.CategoryId == other.CategoryId && this.Price == other.Price && this.IsDiscountActive == other.IsDiscountActive &&
                this.DiscountPrice == other.DiscountPrice && this.Status == other.Status && this.ImageUrl == other.ImageUrl;
        }

        public ProductUpdateRequest ToProductUpdateRequest()
        {
            ProductUpdateRequest productUpdateRequest = new ProductUpdateRequest()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                CategoryId = this.CategoryId,
                Currency = this.Currency,
                Price = this.Price,
                IsDiscountActive = this.IsDiscountActive,
                DiscountPrice = this.DiscountPrice,
                Status = this.Status,
                ImageUrl = this.ImageUrl,
                IsPopular = this.IsPopular,
                Characteristics = this.Characteristics.Select(characteristic => characteristic.Value).ToList()
            };

            return productUpdateRequest;
        }
    }

    public static class ProductExt
    {
        public static ProductResponse ToProductResponse(this Product product)
        {
            ProductResponse productResponse = new ProductResponse()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Category = product.Category,
                Currency = product.Currency,
                Price = product.Price,
                IsDiscountActive = product.IsDiscountActive,
                DiscountPrice = product.DiscountPrice,
                Status = product.Status,
                ImageUrl = product.ImageUrl,
                IsPopular = product.IsPopular
            };

            return productResponse;
        }
    }
}
