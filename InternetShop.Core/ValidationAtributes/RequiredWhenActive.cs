using InternetShop.Core.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.ValidationAtributes
{
    public class RequiredWhenActive : ValidationAttribute
    {
        public string ErrorIncorrectFormat { get; set; } =
            "Неправильний формат продукту";
        public string ErrorMessageWhenActive { get; set; } = 
            "Знижка активна, але ціна зі знижкою не вказана";
        public string ErrorMessageWhenDiscountIsBigger { get; set; } =
            "Ціна зі знижкою вище основної ціни";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance as ProductUpdateRequest;

            if (instance == null)
                return new ValidationResult(ErrorIncorrectFormat);

            if (instance.IsDiscountActive == true && instance.DiscountPrice == null)
                return new ValidationResult(this.ErrorMessageWhenActive);

            if (instance.DiscountPrice > instance.Price)
                return new ValidationResult(this.ErrorMessageWhenDiscountIsBigger);


            return ValidationResult.Success;
        }
    }
}
