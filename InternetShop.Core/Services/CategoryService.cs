using InternetShop.Core.Domain.Entities;
using InternetShop.Core.Domain.RepositoryContracts;
using InternetShop.Core.DTO;
using InternetShop.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponse> AddCategory(CategoryAddRequest categoryAddRequest)
        {
            if(categoryAddRequest == null) 
                throw new ArgumentNullException(nameof(categoryAddRequest));

            if (_categoryRepository.GetAll().Any(category => category.Name == categoryAddRequest.Name))
                throw new ArgumentException("Категорія з таким ім'ям вже існує.");

            //Filtering characteristics
            categoryAddRequest.CharacteristicsList = categoryAddRequest.CharacteristicsList
                .Distinct()
                .Where(characteristic => !string.IsNullOrEmpty(characteristic))
                .ToList();
            
            Category category = categoryAddRequest.ToCategory();
            category.Id = Guid.NewGuid();

            Category categoryAdded = _categoryRepository.Create(category);

            await _categoryRepository.Save();

            return categoryAdded.ToCategoryResponse();
        }

        public async Task<bool> DeleteCategory(Guid id)
        {
            
            bool result = _categoryRepository.Delete(id);

            if (result)
                await _categoryRepository.Save();

            return result;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllCategories()
        {
            IEnumerable<CategoryResponse> categories = _categoryRepository.GetAll()
                .Select(category => category.ToCategoryResponse());

            return categories;
        }

        public async Task<CategoryResponse?> GetCategoryById(Guid id)
        {
            Category? category = _categoryRepository.GetValueById(id);

            if (category == null)
                return null;

            return category.ToCategoryResponse();
        }

        public async Task<IEnumerable<CategoryResponse>> GetFilteredCategories(Expression<Func<CategoryResponse, bool>> predicate)
        {
            IEnumerable<CategoryResponse> categories = await GetAllCategories();

            return categories.AsQueryable().Where(predicate);
        }

        public async Task<CategoryResponse?> UpdateCategory(CategoryUpdateRequest categoryUpdateRequest)
        {
            if(categoryUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(categoryUpdateRequest));
            }

            Category? categorySearched = _categoryRepository.GetAll()
                .FirstOrDefault(category => category.Name == categoryUpdateRequest.Name);

            if (categorySearched != null && categorySearched.Id != categoryUpdateRequest.Id)
                throw new ArgumentException("Категорія з таким ім'ям вже існує.");

            Category categoryToUpdate = categoryUpdateRequest.ToCategory();

            Category? categoryUpdated = _categoryRepository.Update(categoryToUpdate);

            if (categoryUpdated == null)
                return null;

            await _categoryRepository.Save();

            return categoryUpdated.ToCategoryResponse();
        }

        public async Task<bool> VerifyName(string name)
        {
            IEnumerable<Category> categories = _categoryRepository.GetAll();

            return categories.FirstOrDefault(category => category.Name == name) != null;
        }
    }
}
