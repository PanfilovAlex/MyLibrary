using System;
using System.Linq;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.Data.Models;
using System.Text.RegularExpressions;

namespace WebApiMyLib.BLL.Services
{
    public class CategoryValidationService : IValidationService<Category>
    {
        private string pattern = "^[a-zA-zа-яА-Я ]+$";
        public ValidationResult Validate(Category category)
        {
            var _validationResult = new ValidationResult();

            if(category.Name.Trim().Length == 0)
            {
                _validationResult.AddError("Name", "Name is requared");
            }
            if(!Regex.IsMatch(category.Name.Trim(), pattern))
            {
                _validationResult.AddError("Name", "Name should contain letters");
            }
           
            return _validationResult;
        }
    }
}
