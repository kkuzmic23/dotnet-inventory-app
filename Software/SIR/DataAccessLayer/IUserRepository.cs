using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IUserRepository
    {
        User GetByUsername(string username);
        void UpdatePasswordHash(int userId, string passwordHash);
    }
}
