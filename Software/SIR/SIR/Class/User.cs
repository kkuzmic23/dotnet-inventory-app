using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIR.Class
{
    internal class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public User(string firstName, string lastName, string username, string password, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;
            Email = email;
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
