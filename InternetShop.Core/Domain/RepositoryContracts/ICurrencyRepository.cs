using InternetShop.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.Domain.RepositoryContracts
{
    public interface ICurrencyRepository : IRepository<Currency>
    {
        Currency? GetByName(string name);
    }
}
