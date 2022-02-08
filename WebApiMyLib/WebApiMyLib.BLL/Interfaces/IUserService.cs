using WebApiMyLib.Data.Models;

namespace WebApiMyLib.BLL.Interfaces
{
    public interface IUserService
    {
        User GetUser(UserModel model);
    }
}
