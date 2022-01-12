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
        public void Add_ShouldReturnNull_IfRepositoryAddThrewException()
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
            var result = categoryService.Add(category);

            // Assert
            Assert.Null(result);
            categoryRepositoryMock.Verify(m => m.Add(category), Times.Once);
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
    }
}
