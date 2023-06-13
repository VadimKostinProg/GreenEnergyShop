using InternetShop.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.DTO
{
    public class CategoryUpdateRequest
    {
        [Required(ErrorMessage = "Ідентифікатор категорії не вказаний.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Ім'я категорії є обов'язковим полем.")]
        [MaxLength(50, ErrorMessage = "Довжина ім'я категорії не має перевищувати 50 символів.")]
        public string? Name { get; set; }

        public List<string> CharacteristicsList { get; set; } = new List<string>();

        public Category ToCategory()
        {
            Category category = new Category()
            {
                Id = this.Id,
                Name = this.Name,
                CharacteristicsList = string.Join(";", this.CharacteristicsList)
            };

            return category;
        }
    }
}
