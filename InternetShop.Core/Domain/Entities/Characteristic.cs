using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.Domain.Entities
{
    public class Characteristic
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}
