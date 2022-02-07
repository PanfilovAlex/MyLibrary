using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BookDbContext _userRepository;

        public UserRepository(BookDbContext userRepository)
        {
            _userRepository = userRepository;
            if (!_userRepository.Users.Any())
            {
                _userRepository.Add(new User
                {
                    UserName = "Admin",
                    Password = "1234",
                    Role = "admin"
                });
            }
        }

        public User Add(User user)
        {
            var newUser = user;
            _userRepository.Add(user);
            _userRepository.SaveChanges();

            return newUser;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.Users.ToList();
        }

        public User GetUser(UserModel userModel)
        {
            return _userRepository.Users.Where(x => x.UserName.ToLower() == userModel.UserName.ToLower()
            && x.Password == userModel.Password).FirstOrDefault();
        }
    }
}
