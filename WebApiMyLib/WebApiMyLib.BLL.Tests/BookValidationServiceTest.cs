using Xunit;
using WebApiMyLib.BLL.Services;
using WebApiMyLib.Data.Models;
using System.Collections.Generic;


namespace WebApiMyLib.BLL.Tests
{
    public class BookValidationServiceTest
    {
        [Fact]
        public void Validate_ReturnsTrue_IfBookIsValid()
        {
            //Arrange
            var bookValidationService = new BookValidationService();
            var book = new Book()
            {
                Title = "Проверка валидности книги",
            };

            //Act
            bool isValid = bookValidationService.Validate(book).IsValid;

            //Assert
            Assert.True(isValid);
        }

        [Fact]
        public void Validate_ReturnsFalse_IfBookIsNotValid()
        {
            //Arrange
            var bookValidationService = new BookValidationService();
            var book = new Book()
            {
                Title = ""
            };

            //Act
            bool isValid = bookValidationService.Validate(book).IsValid;

            //Assert
            Assert.False(isValid);
        }

    }
}
