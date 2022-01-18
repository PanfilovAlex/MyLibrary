using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.Data.Models;
using WebApiMyLib.BLL.Services;
using WebApiMyLib.Data.Repositories;

namespace WebApiMyLib.BLL.Tests
{
    public class AuthorServiceTest
    {
        [Fact]
        public void Add_ShouldThrowValidationException_IfAuthorIsNotValid()
        {
            // Arrange
            var invalidAuthorValidationResult = new ValidationResult();
            invalidAuthorValidationResult.AddError("Error", "ErrorMessage");
            var authorValidationServiceMock = new Mock<IValidationService<Author>>();
            authorValidationServiceMock
                .Setup(m => m.Validate(It.IsAny<Author>()))
                .Returns(invalidAuthorValidationResult);

            var authorService = new AuthorService(null, authorValidationServiceMock.Object);

            // Act
            Action act = () => authorService.Add(new Author());

            // Assert
            Assert.Throws<ValidationException>(act);
        }

        [Fact]
        public void Add_ShouldReturnAuthor_IfAuthorWasAddedSuccessfully()
        {
            //Arrange
            var authorToAdd = new Author()
            {
                FirstName = "FirstNameTest",
                LastName = "LastNameTest"
            };
            var authorValidationServiceMock = new Mock<IValidationService<Author>>();
            authorValidationServiceMock
                .Setup(m => m.Validate(It.IsAny<Author>()))
                .Returns(new ValidationResult());

            var authorRepositoryMock = new Mock<IAuthorRepository>();
            authorRepositoryMock
                .Setup(m => m.Add(authorToAdd))
                .Returns(authorToAdd);

            var AuthorService = new AuthorService(authorRepositoryMock.Object,
                authorValidationServiceMock.Object);

            // Act
            var result = AuthorService.Add(authorToAdd);

            //Assert
            Assert.Equal(result, authorToAdd);
        }

        [Fact]
        public void Add_ShouldThrowException_IfRepositoryAuthorWasNotAdded()
        {
            // Arrange
            var authorValidationServiceMock = new Mock<IValidationService<Author>>();
            authorValidationServiceMock
                .Setup(m => m.Validate(It.IsAny<Author>()))
                .Returns(new ValidationResult());

            var authorRepositoryMock = new Mock<IAuthorRepository>();
            authorRepositoryMock
                .Setup(m => m.Add(It.IsAny<Author>()))
                .Throws(new Exception());

            var authorService = new AuthorService(authorRepositoryMock.Object,
                authorValidationServiceMock.Object);
            var author = new Author();

            // Act
            Action action = () => authorService.Add(author);

            // Assert
            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void Delete_ShouldThrowException_IfAuthorNotExists()
        {
            // Arrange
            var authorRepositoryServiceMock = new Mock<IAuthorRepository>();
            authorRepositoryServiceMock
                .Setup(m => m.GetAuthors)
                .Throws(new Exception());

            var authorService = new AuthorService(authorRepositoryServiceMock.Object, null);

            // Act 
            Action action = () => authorService.Delete(10);

            // Assert
            authorRepositoryServiceMock.Verify(m => m.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void Find_ShouldThrowException_IfRepositoryReturnsNull()
        {
            // Arrange
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            authorRepositoryMock
                .Setup(m => m.Find(It.IsAny<int>()))
                .Returns((Author)null);

            var authorService = new AuthorService(authorRepositoryMock.Object, null);

            // Act
            Action action = () => authorService.Find(10);

            // Assert
            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void Find_ShouldReturnAuthor_IfAuthorExists()
        {
            // Arrange
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            authorRepositoryMock
                .Setup(m => m.Find(It.IsAny<int>()))
                .Returns(TestAuthor());

            var authorService = new AuthorService(authorRepositoryMock.Object, null);

            // Act
            var result = authorService.Find(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(TestAuthor().Id, result.Id);
            Assert.Equal(TestAuthor().FirstName, result.FirstName);
            Assert.Equal(TestAuthor().LastName, result.LastName);
        }

        private Author TestAuthor()
        {
            return new Author()
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Test"
            };
        }

    }
}
