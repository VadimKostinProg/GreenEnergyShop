using InternetShop.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.DTO
{
    /// <summary>
    /// DTO object of Category entity for add request.
    /// </summary>
    public class CategoryAddRequest
    {
        [Required(ErrorMessage = "Ім'я категорії є обов'язковим полем.")]
        [MaxLength(50, ErrorMessage = "Довжина ім'я категорії не має перевищувати 50 символів.")]
        public string? Name { get; set; }
        
        public List<string> CharacteristicsList { get; set; } = new List<string>();

        public Category ToCategory()
        {
            Category category = new Category()
            {
                Name = this.Name,
                CharacteristicsList = string.Join(";", this.CharacteristicsList)
            };

            return category;
        }
    }
}