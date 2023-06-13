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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryService _categoryService;
        private readonly IImageService _imageService;
        private readonly ICurrencyService _currencyService;

        public ProductService(IProductRepository productRepository, ICategoryService categoryService, 
            IImageService imageService, ICurrencyService currencyService)
        {
            _productRepository = productRepository;
            _categoryService = categoryService;
            _imageService = imageService;
            _currencyService = currencyService;
        }

        public async Task<ProductResponse> AddProduct(ProductAddRequest productAddRequest)
        {
            if(productAddRequest == null) 
                throw new ArgumentNullException(nameof(productAddRequest));

            //Validate for existance of category
            if (await _categoryService.GetCategoryById(productAddRequest.CategoryId) is null)
                throw new KeyNotFoundException("Обраної категорії не існує.");

            if(_productRepository.GetAll().FirstOrDefault(product => product.Name == productAddRequest.Name) != null)
                throw new ArgumentException("Продукт з таким ім'ям вже існує.");

            Product product = productAddRequest.ToProduct();
            product.Id = Guid.NewGuid();

            Product productCreated = _productRepository.Create(product);

            await _productRepository.Save();

            return ConvertToProductResponse(productCreated, convertPrice: false);
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            Product? product = _productRepository.GetValueById(id);

            if (product != null)
                _imageService.DeleteImage(product.ImageUrl);

            bool result = _productRepository.Delete(id);

            if(result)
                await _productRepository.Save();

            return result;
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProducts(bool convertPrice = false)
        {
            return _productRepository.GetAll().Select(product => ConvertToProductResponse(product, convertPrice));
        }

        public async Task<IEnumerable<ProductResponse>> GetFilteredProducts(Expression<Func<ProductResponse, bool>> predicate, bool convertPrice = false)
        {
            IEnumerable<ProductResponse> products = _productRepository.GetAll()
                .Select(product => ConvertToProductResponse(product, convertPrice));

            return products.AsQueryable().Where(predicate);
        }

        public async Task<ProductResponse?> GetProductById(Guid id, bool convertPrice = false)
        {
            Product? product = _productRepository.GetValueById(id);

            if(product == null) 
                return null;

            return ConvertToProductResponse(product, convertPrice);
        }

        public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
        {
            if (productUpdateRequest == null)
                throw new ArgumentNullException(nameof(productUpdateRequest));

            if (await _categoryService.GetCategoryById(productUpdateRequest.CategoryId) is null)
                throw new KeyNotFoundException("Обраної категорії не існує.");

            Product? productSearched = _productRepository.GetAll()
                .FirstOrDefault(product => product.Name == productUpdateRequest.Name);

            if (productSearched != null && productSearched.Id != productUpdateRequest.Id)
                throw new ArgumentException("Продукт з таким ім'ям вже існує.");

            Product product = productUpdateRequest.ToProduct();

            Product? productUpdated = _productRepository.Update(product);

            if (productUpdated == null)
                return null;

            await _productRepository.Save();

            return productUpdated.ToProductResponse();
        }

        public async Task<bool> VerifyName(string name)
        {
            IEnumerable<Product> products = _productRepository.GetAll();

            return products.FirstOrDefault(p => p.Name == name) != null;
        }

        private ProductResponse ConvertToProductResponse(Product product, bool convertPrice)
        {
            ProductResponse response = product.ToProductResponse();

            if (convertPrice)
            {
                switch (product.Currency)
                {
                    case "USD":
                        response.Price = _currencyService.ConvertToUAH(Enums.CurrencyName.USD, product.Price); 
                        if(response.IsDiscountActive) response.DiscountPrice = _currencyService.ConvertToUAH(Enums.CurrencyName.USD, product.DiscountPrice.Value);
                        break;
                    case "EUR":
                        response.Price = _currencyService.ConvertToUAH(Enums.CurrencyName.EUR, product.Price);
                        if (response.IsDiscountActive) response.DiscountPrice = _currencyService.ConvertToUAH(Enums.CurrencyName.EUR, product.DiscountPrice.Value);
                        break;
                }
            }

            Dictionary<string, string> characteristicsDict = new Dictionary<string, string>();

            List<string> characteristicsList = product.Category.CharacteristicsList.Split(';').ToList();
            List<string> characteristics = product.Characteristics.Split(';').ToList();

            if (characteristicsList.Count != characteristics.Count)
                throw new ArgumentException("Список характеристик категорії та продукту не сходитьтся.");

            for (int i = 0; i < characteristicsList.Count; i++)
            {
                characteristicsDict.Add(characteristicsList[i], characteristics[i]);
            }

            response.Characteristics = characteristicsDict;

            return response;
        }
    }
}
