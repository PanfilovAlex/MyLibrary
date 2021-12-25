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
            if(!category.Name.Trim().All(char.IsLetter))
            {
                _validationResult.AddError("Name", "Name should contain letters");
            }
            if(category.Name.Trim().Length < 5)
            {
                _validationResult.AddError("Name", "Name too short");
            }
            return _validationResult;
        }
    }
}
