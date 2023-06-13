using InternetShop.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Data access tools for order header.
    /// </summary>
    public interface IOrderHeaderRepository : IRepository<OrderHeader> { }
}
