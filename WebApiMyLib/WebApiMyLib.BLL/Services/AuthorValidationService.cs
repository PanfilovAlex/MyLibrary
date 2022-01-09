using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.Data.Models;
using System.Linq;

namespace WebApiMyLib.BLL.Services
{
    public class AuthorValidationService : IValidationService<Author>
    {
        private ValidationResult _validationResul = new ValidationResult();
        public ValidationResult Validate(Author author)
        {
            if (author == null)
            {
                _validationResul.AddError("Author", "Author is empty");
            }
            if (author.FirstName.Trim().Length == 0)
            {
                _validationResul.AddError("First Name", "Name can't be empty");
            }
            if (author.FirstName.Trim().Length < 2 || author.FirstName.Trim().Length > 50)
            {
                _validationResul.AddError("First Name", "Name should be more than 2 symbols and lesser than 50 symbols");
            }
            if (author.FirstName.Trim().Any(char.IsNumber) || author.FirstName.Trim().Any(char.IsSymbol))
            {
                _validationResul.AddError("Name", "Name shyould contain letters");
            }
            if (author.LastName.Trim().Length == 0)
            {
                _validationResul.AddError("Last Name", "Last Name cant' be empty");
            }
            if(author.LastName.Trim().Any(char.IsNumber) || author.LastName.Trim().Any(char.IsSymbol))
            {
                _validationResul.AddError("Last Name", "Last Name should contain letters");
            }
            if (author.LastName.Trim().Length < 2 || author.LastName.Trim().Length > 50)
            {
                _validationResul.AddError("Last Name", "Last Name should be more than 2 symbols and lesser than 50 symbols");
            }

            return _validationResul;
        }
    }
}
