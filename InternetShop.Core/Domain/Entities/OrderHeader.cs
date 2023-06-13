using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.Domain.Entities
{
    public class OrderHeader
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Phone]
        [MaxLength(20)]
        public string CustomerPhoneNumber { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string TimeToCall { get; set; } = null!;

        [Required]
        public DateTime OrderConfirmationTime { get; set; }

        public bool Confirmed { get; set; } = false;
    }
}
