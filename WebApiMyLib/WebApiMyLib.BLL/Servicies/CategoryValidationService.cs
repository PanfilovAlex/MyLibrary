using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.BLL.Servicies
{
    public class CategoryValidationService:ICategoryValidationService
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
            return _validationResult;
        }
    }
}
