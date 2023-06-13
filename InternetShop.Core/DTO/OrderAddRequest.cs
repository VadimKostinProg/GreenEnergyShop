using InternetShop.Core.ValidationAtributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.DTO
{
    public sealed class OrderAddRequest
    {
        [Required(ErrorMessage = "Номер замовника обов'язковий")]
        [Phone(ErrorMessage = "Неправильний формат номеру")]
        [MaxLength(20, ErrorMessage = "Неправильний формат номеру")]
        public string CustomerPhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Час зручного дзвінку обов'язковий")]
        [MaxLength(20, ErrorMessage = "Неправильний формат часу")]
        public string TimeToCall { get; set; } = null!;

        [Required]
        public List<Guid> ProductsId { get; set; } = new List<Guid>();

        [Required]
        public List<int> Quantities { get; set; } = new List<int>();
    }
}
