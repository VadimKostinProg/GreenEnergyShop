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
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Product Create(Product entity)
        {
            _db.Products.Add(entity);

            return entity;
        }

        public bool Delete(Guid id)
        {
            Product? productToDelete = _db.Products
                .FirstOrDefault(product => product.Id == id);

            if (productToDelete == null)
                return false;

            _db.Products.Remove(productToDelete);

            return true;
        }

        public IEnumerable<Product> GetAll()
        {
            return _db.Products.Include("Category").ToList();
        }

        public Product? GetValueById(Guid id)
        {
            Product? productSearched = _db.Products.Include("Category")
                .FirstOrDefault(product => product.Id == id);

            return productSearched;
        }

        public Product? Update(Product entity)
        {
            Product? productToUpdate = _db.Products
                .FirstOrDefault(product => product.Id == entity.Id);

            if(productToUpdate == null)
                return null;

            productToUpdate.Name = entity.Name;
            productToUpdate.Description = entity.Description;
            productToUpdate.CategoryId = entity.CategoryId;
            productToUpdate.Currency = entity.Currency;
            productToUpdate.Price = entity.Price;
            productToUpdate.IsDiscountActive = entity.IsDiscountActive;
            productToUpdate.DiscountPrice = entity.DiscountPrice;
            productToUpdate.Status = entity.Status;
            productToUpdate.ImageUrl = entity.ImageUrl;
            productToUpdate.IsPopular = entity.IsPopular;
            productToUpdate.Characteristics = entity.Characteristics;

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
