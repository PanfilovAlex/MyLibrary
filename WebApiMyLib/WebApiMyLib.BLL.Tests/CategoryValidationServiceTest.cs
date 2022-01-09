using System;
using Xunit;
using WebApiMyLib.BLL.Services;
using WebApiMyLib.Data.Models;
using Microsoft.AspNetCore.Http;

namespace WebApiMyLib.BLL.Tests
{
    public class CategoryValidationServiceTest
    {
        [Fact]
        public void CanValidateCategory()
        {
            var categoryValidationResult = new CategoryValidationService();
            var category = new Category()
            {
                Name = "Проверка юнит-тестов",
            };
            
            var categoryForValidation = categoryValidationResult.Validate(category);

            Assert.Empty(categoryForValidation.Errors);
        }
    }
}
