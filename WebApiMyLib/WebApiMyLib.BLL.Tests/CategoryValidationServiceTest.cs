using Xunit;
using WebApiMyLib.BLL.Services;
using WebApiMyLib.Data.Models;


namespace WebApiMyLib.BLL.Tests
{
    public class CategoryValidationServiceTest
    {
        [Fact]
        public void Validate_ReturnsTrue_IfCategoryIsValid()
        {
            //Arrange
            var categoryValidationService = new CategoryValidationService();
            var category = new Category()
            {
                Name = "Проверка юнит-тестов",
            };

            //Act
            bool isValid = categoryValidationService.Validate(category).IsValid;

            //Assert
            Assert.True(isValid);
        }

        [Fact]
        public void Validate_ReturnsFalse_IfCategoryIsEmptyString()
        {
            //Arrange
            var categoryValidationService = new CategoryValidationService();
            var category = new Category()
            {
                Name = ""
            };

            //Act
            bool isValid = categoryValidationService.Validate(category).IsValid;

            //Assert
            Assert.False(isValid);
        }

        [Fact]
        public void Vaidate_ReturnsFalse_IfCategoryContainsSymbols()
        {
            //Arrange
            var categoryValidationService = new CategoryValidationService();
            var category = new Category()
            {
                Name = "Проверка теста на валидно$ть с символами"
            };

            //Act
            bool isValid = categoryValidationService.Validate(category).IsValid;

            //Assert
            Assert.False(isValid);
        }

    }
}
