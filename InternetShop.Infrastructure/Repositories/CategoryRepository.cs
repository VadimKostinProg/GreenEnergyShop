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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Method to create new category entity in data base.
        /// </summary>
        /// <param name="entity">Category object to create.</param>
        /// <returns>Created Category object in data base.</returns>
        public Category Create(Category entity)
        {
            _db.Categories.Add(entity);

            return entity;
        }

        /// <summary>
        /// Method for deleting Category entity from data base.
        /// </summary>
        /// <param name="id">Guid of Category entity to delete.</param>
        /// <returns>If deleting is successful it returns true, otherwise - false.</returns>
        public bool Delete(Guid id)
        {
            Category? categoryToDelete = _db.Categories.FirstOrDefault(category => category.Id == id);

            if(categoryToDelete == null)
            {
                return false;
            }

            _db.Categories.Remove(categoryToDelete);

            return true;
        }

        /// <summary>
        /// Method to read all Category entities from data base.
        /// </summary>
        /// <returns>Collection IEnumerable of Category objects.</returns>
        public IEnumerable<Category> GetAll()
        {
            return _db.Categories.ToList();
        }

        /// <summary>
        /// Method for reading Category entity by it`s Guid.
        /// </summary>
        /// <param name="id">Guid of Category entity to read.</param>
        /// <returns>Category entity with passed guid, null - if Category entity wth passed guid does not exist in data base.</returns>
        public Category? GetValueById(Guid id)
        {
            Category? categorySearched = _db.Categories.FirstOrDefault(category => category.Id == id);

            return categorySearched;
        }

        /// <summary>
        /// Method for updating Category entity in data base.
        /// </summary>
        /// <param name="entity">Category entity to update.</param>
        /// <returns>Updated Category entity.</returns>
        public Category? Update(Category entity)
        {
            Category? categoryToUpdate = _db.Categories
                .FirstOrDefault(category => category.Id == entity.Id);

            if(categoryToUpdate == null)
            {
                return null;
            }

            categoryToUpdate.Name = entity.Name;

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
