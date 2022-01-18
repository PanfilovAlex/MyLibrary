using Xunit;
using Moq;
using WebApiMyLib.BLL.Services;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.Data.Models;
using System;
namespace WebApiMyLib.BLL.Tests
{
    public class BookServiceTest
    {
        [Fact]
        public void Add_ShouldThrowException_IfBookNotValid()
        {
            // Arrange
            var invalidValidationResult = new ValidationResult();
            invalidValidationResult.AddError("bookField", "ErrorMessage");

            var bookValidationExceptionMock = new Mock<IValidationService<Book>>();
            bookValidationExceptionMock
                .Setup(m => m.Validate(It.IsAny<Book>()))
                .Returns(invalidValidationResult);

            var bookService = new BookService(null, bookValidationExceptionMock.Object);
            var book = new Book();
            // Act 
            Action action = () => bookService.Add(book);

            // Assert
            Assert.Throws<ValidationException>(action);
        }
    }
}
