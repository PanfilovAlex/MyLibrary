using Xunit;
using WebApiMyLib.BLL.Services;
using WebApiMyLib.Data.Models;
using System.Collections.Generic;

namespace WebApiMyLib.BLL.Tests
{
    public class AuthorValidationServiceTest
    {

        [Theory]
        [InlineData("Адам", "Фримен", true)]
        [InlineData("", "Фримен", false)]
        [InlineData("Адам", "", false)]
        [InlineData("", "", false)]
        [InlineData("Ада1м", "Фримен", false)]
        [InlineData("Адам", "Ф1римен", false)]
        [InlineData("Ада$м", "Фр$имен", false)]
        [InlineData("Ада:м", "Фримен", false)]
        [InlineData("Адам", "Фр$имен", false)]
        
        public void ValidatAuthor(string firstName, string lastName, bool expectedResult)
        {
            //Arrange
            var authorValidationService = new AuthorValidationService();
            var author = new Author
            {
                FirstName = firstName,
                LastName = lastName
            };

            //Act
            bool isValid = authorValidationService.Validate(author).IsValid;

            //Assert
            Assert.Equal(expectedResult, isValid);
        }
    }
}
