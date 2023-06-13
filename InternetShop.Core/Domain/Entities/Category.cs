using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.Domain.Entities
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Ім'я категорії є обов'язковим полем.")]
        [MaxLength(50, ErrorMessage = "Довжина ім'я категорії не має перевищувати 50 символів.")]
        public string? Name { get; set; }

        public string CharacteristicsList { get; set; } = string.Empty;
    }
}
