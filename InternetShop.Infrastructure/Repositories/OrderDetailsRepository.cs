using InternetShop.Core.Domain.Entities;
using InternetShop.Core.Domain.RepositoryContracts;
using InternetShop.Infrastructure.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Infrastructure.Repositories
{
    public class OrderDetailsRepository : IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderDetailsRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public OrderDetails Create(OrderDetails entity)
        {
            _db.OrderDetails.Add(entity);

            return entity;
        }

        public bool Delete(Guid id)
        {
            var orderDetails = _db.OrderDetails.FirstOrDefault(x => x.Id == id);

            if (orderDetails == null)
                return false;

            _db.OrderDetails.Remove(orderDetails);

            return true;
        }

        public IEnumerable<OrderDetails> GetAll()
        {
            return _db.OrderDetails
                .Include(x => x.OrderHeader)
                .Include(x => x.Product)
                .ToList();
        }

        public IEnumerable<OrderDetails> GetByOrderHeaderId(Guid orderHeaderId)
        {
            return _db.OrderDetails
                .Include(x => x.OrderHeader)
                .Include(x => x.Product)
                .ThenInclude(x => x.Category)
                .Where(x => x.OrderHeaderId == orderHeaderId)
                .ToList();
        }

        public OrderDetails? GetValueById(Guid id)
        {
            return _db.OrderDetails
                .Include(x => x.OrderHeader)
                .Include(x => x.Product)
                .FirstOrDefault(x => x.Id == id);
        }

        public OrderDetails? Update(OrderDetails entity)
        {
            var orderDetailsToUpdate = _db.OrderDetails.FirstOrDefault(x => x.Id == entity.Id);

            if (orderDetailsToUpdate == null)
                return null;

            orderDetailsToUpdate.OrderHeaderId = entity.OrderHeaderId;
            orderDetailsToUpdate.ProductId = entity.ProductId;
            orderDetailsToUpdate.Count = entity.Count;

            return entity;
        }

        /// <summary>
        /// Method for saving changes in the data base.
        /// </summary>
        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
