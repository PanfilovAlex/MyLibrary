using WebApiMyLib.Data.Models;
using WebApiMyLib.BLL.Servicies;

namespace WebApiMyLib.BLL.Interfaces
{
    public interface ICategoryValidationService
    {
        ValidationResult Validate(Category category);   
    }
}
