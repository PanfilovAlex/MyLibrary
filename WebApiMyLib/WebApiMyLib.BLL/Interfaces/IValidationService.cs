using WebApiMyLib.BLL.Services;

namespace WebApiMyLib.BLL.Interfaces
{
    public interface IValidationService<T>
    {
        ValidationResult Validate(T item);   
    }
}
