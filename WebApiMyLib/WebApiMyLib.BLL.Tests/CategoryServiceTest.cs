using Moq;
using System;
using System.Collections.Generic;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.BLL.Services;
using WebApiMyLib.Data.Models;
using WebApiMyLib.Data.Repositories;
using Xunit;

namespace WebApiMyLib.BLL.Tests
{
    public class CategoryServiceTest
    {
        [Fact]
        public void Add_ShouldThrowError_IfCategoryIsNotValid()
        {
            // Arrange
            var categoryValidationServiceMock = new Mock<IValidationService<Category>>();
            var invalidValidationResult = new ValidationResult();
            invalidValidationResult.AddError("field name", "error message");
            categoryValidationServiceMock
                .Setup(m => m.Validate(It.IsAny<Category>()))
                .Returns(invalidValidationResult);

            var categoryService = new CategoryService(null, categoryValidationServiceMock.Object);

            // Act/Assert
            Assert.Throws<ValidationException>(() => categoryService.Add(new Category()));

            // Альтернативный способ проверки
            //// Act
            //Action act = () => categoryService.Add(new Category());

            //// Assert
            //Assert.Throws<ValidationException>(act);
        }

        [Fact]
        public void Add_ShouldThrowExceptio_IfRepositoryAddThrowException()
        {
            // Arrange
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock
                .Setup(m => m.Add(It.IsAny<Category>()))
                .Throws(new Exception());

            var categoryValidationServiceMock = new Mock<IValidationService<Category>>();
            categoryValidationServiceMock
                .Setup(m => m.Validate(It.IsAny<Category>()))
                .Returns(new ValidationResult());

            var categoryService = new CategoryService(categoryRepositoryMock.Object, categoryValidationServiceMock.Object);
            var category = new Category();

            // Act
            Action result = () => categoryService.Add(category);

            // Assert
            Assert.Throws<Exception>(result);
        }

        [Fact]
        public void Add_ShouldReturnCategoty_IfRespositiryAddCategory()
        {
            // Arrange
            var category = new Category();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock
                .Setup(m => m.Add(category))
                .Returns(category);

            var categoryValidationServiceMock = new Mock<IValidationService<Category>>();
            categoryValidationServiceMock
                .Setup(m => m.Validate(It.IsAny<Category>()))
                .Returns(new ValidationResult());

            var categoryService = new CategoryService(categoryRepositoryMock.Object,
                categoryValidationServiceMock.Object);

            // Act
            var result = categoryService.Add(category);

            // Assert 
            Assert.Same(result, category);
        }

        [Fact]
        public void Delete_ShouldNotCallRepositoryDelete_IfCategoryNotExists()
        {
            // Arrange
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock
                .Setup(m => m.Categories)
                .Returns(new List<Category>());

            var categoryService = new CategoryService(categoryRepositoryMock.Object, null);

            // Act
            categoryService.Delete(5);

            //Assert 
            categoryRepositoryMock.Verify(m => m.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void Find_ShouldReturnNull_IfRepositoryCantFindCategory()
        {
            // Arrange
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock
               .Setup(m => m.Find(It.IsAny<int>()))
               .Returns((Category)null);

            var categoryService = new CategoryService(categoryRepositoryMock.Object, null);

            // Act
            Action result = () => categoryService.Find(5);

            //Assert
            Assert.Throws<Exception>(result);
        }

        [Fact]
        public void Find_ShouldReturnCategory_IfRepositoryAddCategorySuccesfuly()
        {
            // Arrange
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock
                .Setup(m => m.Find(It.IsAny<int>()))
                .Returns(DemoCategory());

            var categoryService = new CategoryService(categoryRepositoryMock.Object, null);

            // Act
            var result = categoryService.Find(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(DemoCategory().Id, result.Id);
            Assert.Equal(DemoCategory().Name, result.Name);
        }

        [Fact]
        public void Update_ShouldThrowValidException_IfCategoryIsNotValid()
        {
            // Arange
            var categoryValidationServiceMock = new Mock<IValidationService<Category>>();
            var invalidResult = new ValidationResult();
            invalidResult.AddError("TestingError", "ErrorMessage");

            categoryValidationServiceMock
                .Setup(m => m.Validate(It.IsAny<Category>()))
                .Returns(invalidResult);

            var categoryService = new CategoryService(null, categoryValidationServiceMock.Object);
            var category = new Category();
            // Act
            Action action = () => categoryService.Update(category);

            // Assert
            Assert.Throws<ValidationException>(action);
        }

        [Fact]
        public void Update_ShouldReturnCategory_IfRepositoryWasUpdatedSuccesfully()
        {
            // Arrange
            var categoryValidarionServiceMock = new Mock<IValidationService<Category>>();
            categoryValidarionServiceMock
                .Setup(m => m.Validate(It.IsAny<Category>()))
                .Returns(new ValidationResult());

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock
                .Setup(m => m.Update(It.IsAny<Category>()))
                .Returns(DemoCategory());

            var categoryService = new CategoryService(categoryRepositoryMock.Object, 
                categoryValidarionServiceMock.Object);
            
            //Act 
            var result = categoryService.Update(DemoCategory());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(DemoCategory().Id, result.Id);
            Assert.Equal(DemoCategory().Name, result.Name); 
            Assert.Equal(DemoCategory().Books, result.Books);
            Assert.Equal(DemoCategory().IsDeleted, result.IsDeleted);
        }

        [Fact]
        public void Update_ShouldThrowException_IfRepositoryFailsToUpdate()
        {
            // Arrange
            var categoryValidationServiceMock = new Mock<IValidationService<Category>>();
            categoryValidationServiceMock
                .Setup(m => m.Validate(It.IsAny<Category>()))
                .Returns(new ValidationResult());

            var categoryRepository = new Mock<ICategoryRepository>();
            categoryRepository
                .Setup(m => m.Update(It.IsAny<Category>()))
                .Throws(new Exception());

            var categoryService = new CategoryService(categoryRepository.Object, 
               categoryValidationServiceMock.Object);
            
            // Act
            Action action = () => categoryService.Update(DemoCategory());

            //Assert
            Assert.Throws<Exception>(action);
            categoryRepository.Verify(i => i.Update(It.IsAny<Category>()), Times.Once);
        }

        Category DemoCategory()
        {
            return new Category()
            {
                Id = 1,
                Name = "Тест",
                Books = new List<Book>(),
                IsDeleted = false
            };
        }

    }
}
