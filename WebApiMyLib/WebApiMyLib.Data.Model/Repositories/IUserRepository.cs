using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.Data.Repositories
{
    public interface IUserRepository
    {
        User GetUser(UserModel userModel);
        IEnumerable<User> GetAllUsers();
        User Add(User user);

    }
}
