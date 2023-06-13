using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.Domain.Entities
{
    public class Currency
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string CurrencyName { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }
    }
}
