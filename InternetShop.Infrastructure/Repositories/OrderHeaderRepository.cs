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
    public class OrderHeaderRepository : IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public OrderHeader Create(OrderHeader entity)
        {
            _db.OrderHeaders.Add(entity);

            return entity;
        }

        public bool Delete(Guid id)
        {
            var orderHeader = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);

            if (orderHeader == null)
                return false;

            _db.OrderHeaders.Remove(orderHeader);

            return true;
        }

        public IEnumerable<OrderHeader> GetAll()
        {
            return _db.OrderHeaders.ToList();
        }

        public OrderHeader? GetValueById(Guid id)
        {
            return _db.OrderHeaders.FirstOrDefault(x => x.Id == id);
        }

        public OrderHeader? Update(OrderHeader entity)
        {
            var orderHeader = _db.OrderHeaders.FirstOrDefault(x => x.Id == entity.Id);

            if (orderHeader == null)
                return null;

            orderHeader.CustomerPhoneNumber = entity.CustomerPhoneNumber;
            orderHeader.TimeToCall = entity.TimeToCall;
            orderHeader.OrderConfirmationTime = entity.OrderConfirmationTime;

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
