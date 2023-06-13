using InternetShop.Core.ValidationAtributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.DTO
{
    public class OrderResponse
    {
        public Guid Id { get; set; }

        public string CustomerPhoneNumber { get; set; } = null!;

        public string TimeToCall { get; set; } = null!;

        public DateTime OrderConfirmationTime { get; set; }

        public Dictionary<string, int> Details { get; set; } = new Dictionary<string, int>();

        public decimal TotalCost { get; set; }
        public bool Confirmed { get; set; }
    }
}
