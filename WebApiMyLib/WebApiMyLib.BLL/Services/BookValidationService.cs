using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApiMyLib.BLL.Services
{
    public class BookValidationService : IValidationService<Book>
    {
        private ValidationResult _validationResult = new ValidationResult();
        public ValidationResult Validate(Book book)
        {

            if (book.Title.Trim().Length == 0)
            {
                _validationResult.AddError("Title", "Title can't be empty");
            }

            return _validationResult;
        }
    }
}
