using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.ValidationAtributes
{
    public class QuantityPositive : ValidationAttribute
    {
        public string ErrorIncorrectFormat { get; set; } =
            "Неправильний формат замовлення";
        public string ErrorQuantityNotPositive { get; set; } =
            "Кількість продукту в замовленні не може бути менше 1";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var details = value as Dictionary<Guid, int>;

            if (details == null)
                return new ValidationResult(ErrorIncorrectFormat);

            foreach(var detail in details)
            {
                if (detail.Value < 1)
                    return new ValidationResult(ErrorQuantityNotPositive);
            }

            return ValidationResult.Success;
        }
    }
}
