using AutoFixture;
using InternetShop.Core.DTO;
using InternetShop.Core.ServiceContracts;
using InternetShop.Core.Services;
using InternetShop.Infrastructure.DataBaseContext;
using InternetShop.Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Test.ServiceTests
{
    public class ProductServiceTest
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        private readonly Fixture _fixture;

        private readonly Guid SeedCategoryGuid;

        public ProductServiceTest(IWebHostEnvironment webHostEnvironment)
        {
            ////Initialize fixture
            //_fixture = new Fixture();

            ////Intialize services with InMemory data base
            //DbContextOptions options = new DbContextOptionsBuilder()
            //    .UseInMemoryDatabase("InternetShopCollection")
            //    .Options;

            //ApplicationDbContext context = new ApplicationDbContext(options);

            //CategoryRepository categoryRepository = new CategoryRepository(context);

            //_categoryService = new CategoryService(categoryRepository);

            //ProductRepository productRepository = new ProductRepository(context);

            //_productService = new ProductService(productRepository, 
            //    new CategoryService(categoryRepository), new ImageService(webHostEnvironment));

            ////Add seed category
            //CategoryAddRequest categoryAddRequest = _fixture.Create<CategoryAddRequest>();
            //CategoryResponse categoryResponse = _categoryService.AddCategory(categoryAddRequest).Result;

            //SeedCategoryGuid = categoryResponse.Id;
        }

        #region AddProduct
        //When null object is passed to AddProduct method it should return ArgumentNullException
        [Fact]
        public async Task AddProduct_NullObject()
        {
            //Arrange
            ProductAddRequest? productAddRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _productService.AddProduct(productAddRequest);
            });
        }
        //When object with id of not existing category is passed to AddProduct method it should return ArgumentException
        [Fact]
        public async Task AddProduct_InvalidCategoryId()
        {
            //Arrange
            ProductAddRequest? productAddRequest = _fixture.Create<ProductAddRequest>();

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                //Act
                await _productService.AddProduct(productAddRequest);
            });
        }
        //When valid object is passed to AddProduct method it should return object with the same values
        [Fact]
        public async Task AddProduct_ValidObject()
        {
            //Arrange
            ProductAddRequest? productAddRequest = _fixture.Build<ProductAddRequest>()
                .With(product => product.CategoryId, SeedCategoryGuid)
                .Create();

            //Act
            ProductResponse productResponse = await _productService.AddProduct(productAddRequest);

            //Assert
            Assert.NotNull(productResponse);
        }
        //When objects with duplicate name are passed to AddProduct method it should return ArgumentException
        [Fact]
        public async Task AddProduct_Duplicate()
        {
            //Arrange
            ProductAddRequest? productAddRequest1 = _fixture.Build<ProductAddRequest>()
                .With(product => product.CategoryId, SeedCategoryGuid)
                .Create();
            ProductAddRequest? productAddRequest2 = _fixture.Build<ProductAddRequest>()
                .With(product => product.Name, productAddRequest1.Name)
                .With(product => product.CategoryId, SeedCategoryGuid)
                .Create();

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _productService.AddProduct(productAddRequest1);
                await _productService.AddProduct(productAddRequest2);
            });
        }
        #endregion

        #region GetProductById
        //When guid of not existing object is passed to GetProductById method, it should return null
        [Fact]
        public async Task GetProductById_IvalidGuid()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            //Act
            ProductResponse? productResponse = await _productService.GetProductById(id);

            //Assert
            Assert.Null(productResponse);
        }

        //When valid guid is passed to GetProductById method, it should return object with passed guid
        [Fact]
        public async Task GetProductById_ValidGuid()
        {
            //Arrange
            ProductAddRequest productAddRequest = _fixture.Build<ProductAddRequest>()
                .With(product => product.CategoryId, SeedCategoryGuid)
                .Create();
            ProductResponse productExpected = await _productService.AddProduct(productAddRequest);
            Guid id = productExpected.Id;

            //Act
            ProductResponse? productActual = await _productService.GetProductById(id);

            //Assert
            Assert.NotNull(productActual);
            Assert.Equal(productExpected, productActual);
        }
        #endregion

        #region GetFilteredProduct
        //When some predicate is passed to GetFilteredProducts method, it should return collection of products
        //that satisfy this predicate
        [Fact]
        public async Task GetFilteredProduct_ValidFilter()
        {
            //Arrange
            ProductAddRequest productAddRequest1 = _fixture.Build<ProductAddRequest>()
                .With(product => product.Name, "Product 1")
                .With(product => product.CategoryId, SeedCategoryGuid)
                .Create();
            ProductAddRequest productAddRequest2 = _fixture.Build<ProductAddRequest>()
                .With(product => product.Name, "Product 2")
                .With(product => product.CategoryId, SeedCategoryGuid)
                .Create();

            ProductResponse productResponse1 = await _productService.AddProduct(productAddRequest1);
            ProductResponse productResponse2 = await _productService.AddProduct(productAddRequest2);

            //Act
            IEnumerable<ProductResponse> filteredProducts = 
                await _productService.GetFilteredProducts(product => product.Name.Contains("Product"));

            //Assert
            Assert.True(filteredProducts.Count() == 2);
            Assert.Contains(productResponse1, filteredProducts);
            Assert.Contains(productResponse2, filteredProducts);
        }
        #endregion

        #region UpdateProduct
        //When null object is passed to UpdateProduct method it should return ArgumentNullException
        [Fact]
        public async Task UpdateProduct_NullObject()
        {
            //Arrange
            ProductUpdateRequest? productUpdateRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _productService.UpdateProduct(productUpdateRequest);
            });
        }

        //When product with guid of not existing product is passed to UpdateProduct method, it should return null
        [Fact]
        public async Task UpdateProduct_InvalidGuid()
        {
            //Arrange
            ProductUpdateRequest productUpdateRequest = _fixture.Build<ProductUpdateRequest>()
                .With(product => product.CategoryId, SeedCategoryGuid)
                .Create();

            //Act
            ProductResponse? productResponse = await _productService.UpdateProduct(productUpdateRequest);

            //Assert
            Assert.Null(productResponse);
        }
        //When product with duplicate name is passed to UpdateProduct method, it should return ArgumentException
        [Fact]
        public async Task UpdateProduct_Duplicate()
        {
            //Arrange
            ProductAddRequest productAddRequest1 = _fixture.Build<ProductAddRequest>()
                .With(product => product.Name, "Product1")
                .With(product => product.CategoryId, SeedCategoryGuid)
                .Create();
            ProductAddRequest productAddRequest2 = _fixture.Build<ProductAddRequest>()
                .With(product => product.Name, "Product2")
                .With(product => product.CategoryId, SeedCategoryGuid)
                .Create();

            ProductResponse productResponse1 = await _productService.AddProduct(productAddRequest1);
            ProductResponse productResponse2 = await _productService.AddProduct(productAddRequest2);

            ProductUpdateRequest productUpdateRequest = _fixture.Build<ProductUpdateRequest>()
                .With(product => product.Id, productResponse2.Id)
                .With(product => product.Name, "Product1")
                .With(product => product.CategoryId, SeedCategoryGuid)
                .Create();

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _productService.UpdateProduct(productUpdateRequest);
            });
        }
        //When object with id of not existing category is passed to UpdateProduct method it should return ArgumentException
        [Fact]
        public async Task UpdateProduct_InvalidCategoryId()
        {
            //Arrange
            ProductAddRequest productAddRequest = _fixture.Build<ProductAddRequest>()
                .With(product => product.CategoryId, SeedCategoryGuid)
                .Create();

            ProductResponse productResponse = await _productService.AddProduct(productAddRequest);

            ProductUpdateRequest productUpdateRequest = _fixture.Build<ProductUpdateRequest>()
                .With(product => product.Id, productResponse.Id)
                .Create();

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                //Act
                await _productService.UpdateProduct(productUpdateRequest);
            });
        }

        //When valid object is passed to UpdateProduct method it object should be updated and method should return updated object
        [Fact]
        public async Task UpdateProduct_ValidObject()
        {
            //Arrange
            ProductAddRequest productAddRequest = _fixture.Build<ProductAddRequest>()
                .With(product => product.CategoryId, SeedCategoryGuid)
                .Create();

            ProductResponse productExpected = await _productService.AddProduct(productAddRequest);

            ProductUpdateRequest productUpdateRequest = _fixture.Build<ProductUpdateRequest>()
                .With(product => product.Id, productExpected.Id)
                .With(product => product.CategoryId, SeedCategoryGuid)
                .Create();

            //Act
            ProductResponse? productActual = await _productService.UpdateProduct(productUpdateRequest);

            //Assert
            Assert.NotNull(productActual);
            Assert.Equal(productActual, productActual);
        }
        #endregion

        #region DeleteProduct
        //When guid of not existing object is passed to DeleteProduct method, it should return false
        [Fact]
        public async Task DeleteProduct_InvalidGuid()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            //Act
            bool result = await _productService.DeleteProduct(guid);

            //Assert
            Assert.False(result);
        }

        //When valid guid is passed to DeleteProduct method, object should be deleted and
        //method should return true
        [Fact]
        public async Task DeleteProduct_ValidGuid()
        {
            //Arrange
            ProductAddRequest productAddRequest = _fixture.Build<ProductAddRequest>()
                .With(product => product.CategoryId, SeedCategoryGuid)
                .Create();

            ProductResponse productResponse = await _productService.AddProduct(productAddRequest);

            //Act
            bool result = await _productService.DeleteProduct(productResponse.Id);
            ProductResponse? productDeleted = await _productService.GetProductById(productResponse.Id);

            //Assert
            Assert.True(result);
            Assert.Null(productDeleted);
        }
        #endregion
    }
}
