using InternetShop.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Ім'я продукту є обов'язковим")]
        [MaxLength(100, ErrorMessage = "Ім'я продукту має займати до 100 символів")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Опис продукту є обов'язковим")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Категорія продукту є обов'язковою")]
        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [Required(ErrorMessage = "Валюта ціни є обов'язковою")]
        public string Currency { get; set; } = null!;

        [Required(ErrorMessage = "Ціна продукту є обов'язковою")]
        public decimal Price { get; set; }

        public bool IsDiscountActive { get; set; } = false;

        public decimal? DiscountPrice { get; set; }

        public ProductStatus Status { get; set; } = ProductStatus.InStock;

        [Required(ErrorMessage = "Зображення продукту є обов'язковим")]
        public string? ImageUrl { get; set; }

        public bool IsPopular { get; set; }

        public string Characteristics { get; set; } = string.Empty;
    }
}
