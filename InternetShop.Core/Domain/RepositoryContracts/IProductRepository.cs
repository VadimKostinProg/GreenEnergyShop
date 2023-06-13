using InternetShop.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Data acces tools for Product.
    /// </summary>
    public interface IProductRepository : IRepository<Product> { }
}
