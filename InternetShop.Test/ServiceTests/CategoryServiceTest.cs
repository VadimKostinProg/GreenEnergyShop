using InternetShop.Core.Domain.Entities;
using InternetShop.Core.Domain.RepositoryContracts;
using InternetShop.Core.DTO;
using InternetShop.Core.ServiceContracts;
using InternetShop.Core.Services;
using InternetShop.Infrastructure.DataBaseContext;
using InternetShop.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using NuGet.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Test.ServiceTests
{
    public class CategoryServiceTest
    {
        private readonly ICategoryService _categoryService;

        public CategoryServiceTest()
        {
            DbContextOptions options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("InternetShopCollection")
                .Options;
            ApplicationDbContext context = new ApplicationDbContext(options);

            CategoryRepository categoryRepository = new CategoryRepository(context);

            _categoryService = new CategoryService(categoryRepository);
        }

        #region AddCategory
        //When null object is passed to AddCategory method, it should throw ArgumentNullException
        [Fact]
        public async Task AddCategory_NullObject()
        {
            //Arrange
            CategoryAddRequest categoryAddRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _categoryService.AddCategory(categoryAddRequest);
            });
        }

        //When duplicate object is passed to AddCategory method, it should throw ArgumentException
        [Fact]
        public async Task AddCategory_Duplicate()
        {
            //Arrange
            CategoryAddRequest categoryAddRequest1 = new CategoryAddRequest()
            {
                Name = "Сонячні панелі"
            };
            CategoryAddRequest categoryAddRequest2 = new CategoryAddRequest()
            {
                Name = "Сонячні панелі"
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _categoryService.AddCategory(categoryAddRequest1);
                await _categoryService.AddCategory(categoryAddRequest2);
            });
        }

        //When valid object was passed to AddCategory method, it should return the same object
        [Fact]
        public async Task AddCategory_ValidObject()
        {
            //Arrange
            CategoryAddRequest categoryAddRequest = new CategoryAddRequest()
            {
                Name = "Стабілізатори"
            };

            //Act
            CategoryResponse categoryResponse = await _categoryService.AddCategory(categoryAddRequest);

            //Assert
            Assert.Equal(categoryResponse.Name, categoryAddRequest.Name);
        }
        #endregion

        #region GetCategoryById
        //When guid of not existing object is passed to GetCategoryById method, it should return null
        [Fact]
        public async Task GetCategoryById_InvalidGuid()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            //Act
            CategoryResponse? categoryResponse = await _categoryService.GetCategoryById(guid);

            //Assert
            Assert.Null(categoryResponse);
        }

        //When valid guid is passed to GetCategoryById method, it should return apropriate CategoryResponse object
        [Fact]
        public async Task GetCategoryById_ValidGuid()
        {
            //Arrange
            CategoryAddRequest categoryAddRequest = new CategoryAddRequest() { Name = "Лампи" };
            CategoryResponse categoryExpected = await _categoryService.AddCategory(categoryAddRequest);

            //Act
            CategoryResponse? categoryActual = await _categoryService.GetCategoryById(categoryExpected.Id);

            //Assert
            Assert.NotNull(categoryActual);
            Assert.Equal(categoryExpected, categoryActual);
        }
        #endregion

        #region GetFilteredCategories
        //When some predicate is passed to GetFilteredCategories method,
        //it should return collection of categories that satisfy this predicate
        [Fact]
        public async Task GetFilteredCategories_ValidPredicate()
        {
            //Assert
            CategoryAddRequest categoryAddRequest1 = new CategoryAddRequest()
            {
                Name = "Категорія 1"
            };
            CategoryAddRequest categoryAddRequest2 = new CategoryAddRequest()
            {
                Name = "Категорія 2"
            };
            CategoryResponse categoryResponse1 = await _categoryService.AddCategory(categoryAddRequest1);
            CategoryResponse categoryResponse2 = await _categoryService.AddCategory(categoryAddRequest2);

            //Act
            IEnumerable<CategoryResponse> filteredCategories = await _categoryService
                .GetFilteredCategories(category => category.Name.Contains("Категорія"));

            //Assert
            Assert.Contains(categoryResponse1, filteredCategories);
            Assert.Contains(categoryResponse2, filteredCategories);
        }
        #endregion

        #region UpdateCategory
        //When null object is passed to UpdateCategory method, it should throw ArgumentNullException
        [Fact]
        public async Task UpdateCategory_NullObject()
        {
            //Arrange
            CategoryUpdateRequest? categoryUpdateRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _categoryService.UpdateCategory(categoryUpdateRequest);
            });
        }

        //When object with duplicate name is passed to UdpateCategory method, it should throw ArgumentException
        [Fact]
        public async Task UpdateCategory_Duplicate()
        {
            //Arrange
            CategoryAddRequest categoryAddRequest1 = new CategoryAddRequest()
            {
                Name = "Category before updating"
            };
            CategoryAddRequest categoryAddRequest2 = new CategoryAddRequest()
            {
                Name = "Category updated"
            };

            CategoryResponse categoryResponse1 = await _categoryService.AddCategory(categoryAddRequest1);
            CategoryResponse categoryResponse2 = await _categoryService.AddCategory(categoryAddRequest2);

            CategoryUpdateRequest categoryUpdateRequest = new CategoryUpdateRequest()
            {
                Id = categoryResponse1.Id,
                Name = "Category updated"
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _categoryService.UpdateCategory(categoryUpdateRequest);
            });
        }

        //When object with guid of not existing in data store elemen is passed to UpdateCategory method,
        //it should return null
        [Fact]
        public async Task UpdateCategory_InvalidObject()
        {
            //Arrange
            CategoryUpdateRequest categoryUpdateRequest = new CategoryUpdateRequest()
            {
                Id = Guid.NewGuid(),
                Name = "Invalid category"
            };

            //Act
            CategoryResponse? categoryResponse = await _categoryService.UpdateCategory(categoryUpdateRequest);

            //Assert
            Assert.Null(categoryResponse);
        }

        //When object valid object is passed to UdpateCategory element, data store should contain updated object
        [Fact]
        public async Task UpdateCategory_ValidObject()
        {
            //Arrange
            CategoryAddRequest categoryAddRequest = new CategoryAddRequest()
            {
                Name = "Category before updating"
            };
            CategoryResponse categoryResponse = await _categoryService.AddCategory(categoryAddRequest);
            CategoryUpdateRequest categoryUpdateRequest = new CategoryUpdateRequest()
            {
                Id = categoryResponse.Id,
                Name = "Category after updating"
            };

            //Act
            CategoryResponse? categoryUpdated = await _categoryService.UpdateCategory(categoryUpdateRequest);


            //Assert
            List<CategoryResponse> categories = (await _categoryService
                .GetFilteredCategories(category => category.Name == "Category after updating")).ToList();

            Assert.NotNull(categoryResponse);
            Assert.True(categories.Count() > 0);
        }
        #endregion

        #region DeleteCategory
        //When guid of not existing category is passed to DeleteGategory method, it should return false
        [Fact]
        public async Task DleeteCategory_InvalidGuid()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            //Act
            bool result = await _categoryService.DeleteCategory(guid);

            //Assert
            Assert.False(result);
        }

        //When valid guid is passed to DeleteCategory method, it should return true
        [Fact]
        public async Task DleeteCategory_ValidGuid()
        {
            //Arrange
            CategoryAddRequest categoryAddRequest = new CategoryAddRequest()
            {
                Name = "Category to delete"
            };
            CategoryResponse categoryResponse = await _categoryService.AddCategory(categoryAddRequest);

            //Act
            bool result = await _categoryService.DeleteCategory(categoryResponse.Id);

            //Assert
            Assert.True(result);
        }
        #endregion
    }
}
