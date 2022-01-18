using Xunit;
using Moq;
using WebApiMyLib.BLL.Services;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.Data.Models;
using System;
using WebApiMyLib.Data.Repositories;
using System.Collections.Generic;

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

        [Fact]
        public void Add_ShouldReturnCategory_IfBookWasAddedSuccessfully()
        {
            // Arrange
            var book = new Book()
            {
                Title = "Test"
            };
            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock
                .Setup(m => m.AddBook(book))
                .Returns(book);
            var bookVlidationMock = new Mock<IValidationService<Book>>();
            bookVlidationMock
                .Setup(m => m.Validate(It.IsAny<Book>()))
                .Returns(new ValidationResult());   

            var bookService = new BookService(bookRepositoryMock.Object, bookVlidationMock.Object);

            // Act
            var result = bookService.Add(book);

            // Assert
            Assert.Equal(book, result);
        }

        [Fact]
        public void Add_ShouldThrowException_IfBookNotAddedInRepo()
        {
            // Arrange
            var bookRepositoryMock = new Mock <IBookRepository>();
            bookRepositoryMock
                .Setup(m => m.AddBook(It.IsAny<Book>()))
                .Throws(new Exception());

            var bookValidationServiceMock = new Mock<IValidationService<Book>>();
            bookValidationServiceMock
                .Setup(m => m.Validate(It.IsAny<Book>()))
                .Returns(new ValidationResult());

            var bookService = new BookService(bookRepositoryMock.Object, bookValidationServiceMock.Object);
            var book = new Book();
            // Act
            Action action = () => bookService.Add(book);

            // Assert
            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void Delete_ShouldNotCallDelete_IfBookNotExists()
        {
            // Arrange
            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock
                .Setup(m => m.GetBooks)
                .Returns(new List<Book>());

            var bookService = new BookService(bookRepositoryMock.Object, null);

            // Act
            bookService.Delete(5);

            // Assert
            bookRepositoryMock.Verify(i => i.DeleteBook(It.IsAny<int>()), Times.Never());
        }

        [Fact]
        public void Find_ShouldReturnBook_IfBookExists()
        {
            // Arrange
            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock
                .Setup(m => m.Find(It.IsAny<int>()))
                .Returns(TestBook());

            var bookService = new BookService(bookRepositoryMock.Object, null);
            var book = TestBook();

            // Act
            var result = bookService.Find(book.Id);

            // Assert
            Assert.Equal(book.Id, result.Id);
            Assert.Equal(book.Title, result.Title);
        }

        [Fact]
        public void Find_ShouldThrowException_IfBookNotExists()
        {
            // Arrange
            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock
                .Setup(m => m.Find(It.IsAny<int>()))
                .Throws(new Exception());

            var bookService = new BookService(bookRepositoryMock.Object, null);

            // Act
            Action action = () => bookService.Find(5);

            // Assert
            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void Update_ShouldThrowException_IfBookIsNotValid()
        {
            // Arrange
            var invalidValidationResult = new ValidationResult();
            invalidValidationResult.AddError("ErrorField", "ErrorMessage");
            var bookValidationServiceMock = new Mock<IValidationService<Book>>();
            bookValidationServiceMock
                .Setup(m => m.Validate(It.IsAny<Book>()))
                .Returns(invalidValidationResult);

            var bookService = new BookService(null, bookValidationServiceMock.Object);
            var book = TestBook();
            // Act
            Action action = () => bookService.Update(book);

            // Assert
            Assert.Throws<ValidationException>(action);
        }

        [Fact]
        public void Update_ShouldReturnBook_IfBookWasAddedInRepo()
        {
            // Arrange
            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock
                .Setup(m => m.UpdateBook(It.IsAny<Book>()))
                .Returns(TestBook());

            var bookValidationServiceMock = new Mock<IValidationService<Book>>();
            bookValidationServiceMock
                .Setup(m => m.Validate(It.IsAny<Book>()))
                .Returns(new ValidationResult());

            var bookService = new BookService(bookRepositoryMock.Object, bookValidationServiceMock.Object);

            // Act
            var result = bookService.Update(TestBook());

            // Assert
            Assert.Equal(TestBook().Id, result.Id );
            Assert.Equal(TestBook().Title, result.Title );
        }

        private Book TestBook()
        {
            return new Book()
            {
                Id = 100,
                Title = "Test"
            };
        }
    }
}
