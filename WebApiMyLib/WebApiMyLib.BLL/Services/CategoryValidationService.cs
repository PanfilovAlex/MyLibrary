using System;
using System.Linq;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.BLL.Services
{
    public class CategoryValidationService : IValidationService<Category>
    {
        private ValidationResult _validationResult = new ValidationResult();

        public ValidationResult Validate(Category category)
        {
            if (category.Name.Trim().Length == 0)
            {
                _validationResult.AddError("Name", "Name is requared");
            }
            if(category.Name.Trim().Any(char.IsNumber) || category.Name.Trim().Any(char.IsSymbol))
            {
                _validationResult.AddError("Name", "Name should contain letters");
            }
           
            return _validationResult;
        }
    }
}
