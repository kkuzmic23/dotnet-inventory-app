using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccessLayer;
using EntityLayer.Entities;

namespace BusinessLogicLayer
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var user = _userRepository.GetByUsername(username);
            if (user == null)
            {
                return null;
            }

            if (PasswordHasher.IsHashFormat(user.Password))
            {
                return PasswordHasher.Verify(password, user.Password) ? user : null;
            }

            if (user.Password == password)
            {
                var hashed = PasswordHasher.HashPassword(password);
                _userRepository.UpdatePasswordHash(user.Id, hashed);
                user.Password = hashed;
                return user;
            }

            return null;
        }
    }
}
