using InternetShop.Core.Domain.Entities;
using InternetShop.Core.Domain.RepositoryContracts;
using InternetShop.Infrastructure.DataBaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Infrastructure.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly ApplicationDbContext _db;

        public CurrencyRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Currency Create(Currency entity)
        {
            _db.Currencies.Add(entity);

            return entity;
        }

        public bool Delete(Guid id)
        {
            Currency? entity = _db.Currencies.FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            _db.Currencies.Remove(entity);

            return true;
        }

        public IEnumerable<Currency> GetAll()
        {
            return _db.Currencies.ToList();
        }

        public Currency? GetByName(string name)
        {
            return _db.Currencies.FirstOrDefault(x => x.CurrencyName == name);
        }

        public Currency? GetValueById(Guid id)
        {
            return _db.Currencies.FirstOrDefault(x => x.Id == id);
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

        public Currency? Update(Currency entity)
        {
            Currency? currencyToUpdate = _db.Currencies.FirstOrDefault(x => x.Id == entity.Id);
            
            if(currencyToUpdate == null)
            {
                return null;
            }

            currencyToUpdate.CurrencyName = entity.CurrencyName;
            currencyToUpdate.Price = entity.Price;

            return currencyToUpdate;
        }
    }
}
