using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.BLL.Servicies
{
    public class CategoryValidationService
    {
        public static bool IsValid(IValidationDictionary validator, Category category)
        {
            if (category.Name.Trim().Length == 0)
            {
                validator.AddError("Name", "Name is requared");
            }
            if(!category.Name.Trim().All(char.IsLetter))
            {
                validator.AddError("Name", "Name should contain letters");
            }
            return validator.IsValid;
        }
    }
}
