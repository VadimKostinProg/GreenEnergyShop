using InternetShop.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.ServiceContracts
{
    /// <summary>
    /// Service for categories.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Method for adding new category to data source.
        /// </summary>
        /// <param name="categoryAddRequest">CategoryAddRequest object to add.</param>
        /// <returns>CategoryResponse object with details of added to data source category.</returns>
        Task<CategoryResponse> AddCategory(CategoryAddRequest categoryAddRequest);

        /// <summary>
        /// Method for reading category from data source
        /// </summary>
        /// <param name="id">Guid of category to read.</param>
        /// <returns>CategoryResponse object with details of category with passed guid from data source,
        /// null - if category with passed guid does not exist in data source.</returns>
        Task<CategoryResponse?> GetCategoryById(Guid id);

        /// <summary>
        /// Method for reading all categories from data source.
        /// </summary>
        /// <returns>Collection IEnumerable of CategoryResponse objects.</returns>
        Task<IEnumerable<CategoryResponse>> GetAllCategories();

        /// <summary>
        /// Method for reading categories filtered by predicate.
        /// </summary>
        /// <param name="predicate">Expression to filter categories.</param>
        /// <returns>Collection IEnumerable of filtered CategoryResponse object.</returns>
        Task<IEnumerable<CategoryResponse>> GetFilteredCategories(Expression<Func<CategoryResponse, bool>> predicate);

        /// <summary>
        /// Method for updating category in data source.
        /// </summary>
        /// <param name="categoryUpdateRequest">CategoryUpdateRequest with details of category to update.</param>
        /// <returns>CategoryRespons object with details of updated category in data source,
        /// null - if category with passed id does not exist in data source.</returns>
        Task<CategoryResponse?> UpdateCategory(CategoryUpdateRequest categoryUpdateRequest);

        /// <summary>
        /// Method for deleting category from data source.
        /// </summary>
        /// <param name="id">Guid of category to delete.</param>
        /// <returns>True - if deleting is successful, otherwise - false.</returns>
        Task<bool> DeleteCategory(Guid id);

        /// <summary>
        /// Method for verifying category name for already existance in data store.
        /// </summary>
        /// <param name="name">Name of category to verify.</param>
        /// <returns>True - if this name is already present in data store, otherwise - false.</returns>
        Task<bool> VerifyName(string name);
    }
}
