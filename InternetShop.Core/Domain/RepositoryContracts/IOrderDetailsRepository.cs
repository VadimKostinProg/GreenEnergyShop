using InternetShop.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Data access tools for order details.
    /// </summary>
    public interface IOrderDetailsRepository : IRepository<OrderDetails>
    {
        /// <summary>
        /// Method to filter order details by order header.
        /// </summary>
        /// <param name="orderHeaderId">Id of order header to filter details.</param>
        /// <returns>Collection IEnumerable of OrderDetails.</returns>
        IEnumerable<OrderDetails> GetByOrderHeaderId(Guid orderHeaderId);
    }
}
