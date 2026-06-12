using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Entities;

namespace DataAccessLayer
{
    public class UserRepository : IUserRepository
    {
        public User GetByUsername(string username)
        {
            using (var context = new Model1())
            {
                return context.Users.FirstOrDefault(u => u.Username == username);
            }
        }
        public void UpdatePasswordHash(int userId, string passwordHash)
        {
            using (var context = new Model1())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    return;
                }

                user.Password = passwordHash;
                context.SaveChanges();
            }
        }
    }
}