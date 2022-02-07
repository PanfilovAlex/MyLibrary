using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.BLL.Interfaces
{
    public interface IUserService
    {
        User GetUser(UserModel model);
    }
}
