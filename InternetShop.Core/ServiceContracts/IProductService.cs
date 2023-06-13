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
    /// Service for products.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Method to add new product to data source.
        /// </summary>
        /// <param name="productAddRequest">Product to add</param>
        /// <returns>Inserted product</returns>
        Task<ProductResponse> AddProduct(ProductAddRequest productAddRequest);

        /// <summary>
        /// Method for reading product from data source by it`s guid.
        /// </summary>
        /// <param name="id">Guid of product to read.</param>
        /// <returns>Product with passed guid, null - if product with passed guid does not exist in data source.</returns>
        Task<ProductResponse?> GetProductById(Guid id, bool convertPrice = false);

        /// <summary>
        /// Method for reading all pdoructs from data source.
        /// </summary>
        /// <returns>Collection IEnumerable of all products from data source.</returns>
        Task<IEnumerable<ProductResponse>> GetAllProducts(bool convertPrice = false);

        /// <summary>
        /// Method to read products from data source filtered by predicate.
        /// </summary>
        /// <param name="predicate">Predicate to filter products</param>
        /// <returns>Collection IEnumerable of products filtered by passed predicate</returns>
        Task<IEnumerable<ProductResponse>> GetFilteredProducts(Expression<Func<ProductResponse, bool>> predicate, bool convertPrice = false);

        /// <summary>
        /// Method for updating product in data source.
        /// </summary>
        /// <param name="productUpdateRequest">Product to update</param>
        /// <returns>Updated product</returns>
        Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest);

        /// <summary>
        /// Method for deleting product from data source bu it`s guid.
        /// </summary>
        /// <param name="id">Guid of product to delete</param>
        /// <returns>True - if deleting is successful, otherwise - false</returns>
        Task<bool> DeleteProduct(Guid id);

        /// <summary>
        /// Method for verifying product name for already existance in data source.
        /// </summary>
        /// <param name="name">Product name to verify.</param>
        /// <returns>True - if product with such name is present in data source, otherwise - false.</returns>
        Task<bool> VerifyName(string name);
    }
}
