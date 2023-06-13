using InternetShop.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.ServiceContracts
{
    public interface ICurrencyService
    {
        decimal GetPriceInUAH(CurrencyName currency);
        Task SetPriceInUah(decimal price, CurrencyName currency);
        decimal ConvertToUAH(CurrencyName currency, decimal sum);
    }
}
