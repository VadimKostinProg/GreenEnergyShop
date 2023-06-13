using InternetShop.Core.Domain.Entities;
using InternetShop.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetShop.Core.ValidationAtributes;

namespace InternetShop.Core.DTO
{
    public class ProductUpdateRequest
    {
        [Required(ErrorMessage = "Ідентифікатор продукту є обов'язковим")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Ім'я продукту є обов'язковим")]
        [MaxLength(100, ErrorMessage = "Ім'я продукту має займати до 100 символів")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Опис продукту є обов'язковим")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Категорія продукту є обов'язковою")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Валюта ціни продукту є обов'язковою")]
        public string Currency { get; set; } = null!;

        [Required(ErrorMessage = "Ціна продукту є обов'язковою")]
        [Range(0, 1000000)]
        public decimal Price { get; set; }

        public bool IsDiscountActive { get; set; } = false;

        [Range(0, 1000000)]
        [RequiredWhenActive]
        public decimal? DiscountPrice { get; set; }

        public ProductStatus Status { get; set; } = ProductStatus.InStock;

        public string? ImageUrl { get; set; }

        public bool IsPopular { get; set; }

        public List<string> Characteristics { get; set; } = new List<string>();

        public Product ToProduct()
        {
            Product product = new Product()
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
                Characteristics = string.Join(";", this.Characteristics)
            };

            return product;
        }
    }
}
