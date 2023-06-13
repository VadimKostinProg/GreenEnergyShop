using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.Domain.Entities
{
    public class OrderDetails
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid OrderHeaderId { get; set; }

        [ForeignKey("OrderHeaderId")]
        [ValidateNever]
        public OrderHeader? OrderHeader { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product? Product { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Count { get; set; }

        [Required]
        [Range(0, 1000000)]
        public decimal Price { get; set; }
    }
}
