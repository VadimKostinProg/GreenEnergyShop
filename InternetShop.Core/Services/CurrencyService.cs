using InternetShop.Core.Domain.Entities;
using InternetShop.Core.Domain.RepositoryContracts;
using InternetShop.Core.Enums;
using InternetShop.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public decimal ConvertToUAH(CurrencyName currency, decimal sum)
        { 
            decimal price = GetPriceInUAH(currency);

            decimal sumInUAH = price * sum;

            decimal roundedSum = Math.Ceiling(sumInUAH/ 10) * 10;

            return roundedSum;
        }

        public decimal GetPriceInUAH(CurrencyName currency)
        {
            Currency? curr = _currencyRepository.GetByName(currency.ToString());

            if (curr == null)
            {
                throw new KeyNotFoundException("Ціна валюти не знайдена");
            }

            return curr.Price;
        }

        public async Task SetPriceInUah(decimal price, CurrencyName currency)
        {
            Currency? curr = _currencyRepository.GetByName(currency.ToString());

            if (curr == null)
            {
                Currency newCurrency = new Currency() 
                { 
                    Id = Guid.NewGuid(), 
                    CurrencyName = currency.ToString(), 
                    Price = price 
                };

                _currencyRepository.Create(newCurrency);
            }
            else
            {
                curr.Price = price;
                _currencyRepository.Update(curr);
            }

            await _currencyRepository.Save();
        }
    }
}
