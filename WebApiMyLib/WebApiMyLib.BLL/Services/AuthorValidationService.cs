using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.Data.Models;
using System.Linq;
using System.Text.RegularExpressions;

namespace WebApiMyLib.BLL.Services
{
    public class AuthorValidationService : IValidationService<Author>
    {
        private string pattern = "^[a-zA-zа-яА-Я]+$";
        public ValidationResult Validate(Author author)
        {
            var _validationResult = new ValidationResult();

            if(author == null)
            {
                _validationResult.AddError("Author", "Author is empty");
            }
            if(author.FirstName.Trim().Length == 0)
            {
                _validationResult.AddError("First Name", "Name can't be empty");
            }
            if(author.FirstName.Trim().Length < 2 || author.FirstName.Trim().Length > 50)
            {
                _validationResult.AddError("First Name", "Name should be more than 2 symbols and less than 50 symbols");
            }
            if(!Regex.IsMatch(author.FirstName.Trim(), pattern))
            {
                _validationResult.AddError("Name", "Name should contain only letters");
            }
            if(author.LastName.Trim().Length == 0)
            {
                _validationResult.AddError("Last Name", "Last Name cant' be empty");
            }
            if(!Regex.IsMatch(author.LastName.Trim(), pattern))
            {
                _validationResult.AddError("Last Name", "Last Name should contain only letters");
            }
            if(author.LastName.Trim().Length < 2 || author.LastName.Trim().Length > 50)
            {
                _validationResult.AddError("Last Name", "Last Name should be more than 2 symbols and less than 50 symbols");
            }

            return _validationResult;
        }
    }
}
