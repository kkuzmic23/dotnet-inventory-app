using BusinessLogicLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace xUnitTests.IntegrationTests
{
    public class UserIntegrationTests
    {
        private readonly UserRepository repo = new UserRepository();
        private readonly UserService service = new UserService();

        string validUsername = "admin";
        string validPassword = "adminpw123";

        string invalidUsername = "adminn";
        string invalidPassword = "admin123";

        [Fact]
        public void Authenticate_WhenValidCredentials_ReturnsUser()
        {
            var result = service.Authenticate(validUsername, validPassword);

            Assert.NotNull(result);
            Assert.Equal(validUsername, result.Username);
        }

        [Fact]
        public void Authenticate_WhenWrongPassword_ReturnsNull()
        {
            var result = service.Authenticate(validUsername, invalidPassword);

            Assert.Null(result);
        }

        [Fact]
        public void Authenticate_WhenUserDoesNotExist_ReturnsNull()
        {
            var result = service.Authenticate(invalidUsername, validPassword);

            Assert.Null(result);
        }

        [Fact]
        public void Authenticate_WhenUsernameIsNull_ReturnsNull()
        {
            var result = service.Authenticate(null, validPassword);

            Assert.Null(result);
        }

        [Fact]
        public void Authenticate_WhenPasswordIsNull_ReturnsNull()
        {
            var result = service.Authenticate(validUsername, null);

            Assert.Null(result);
        }
    }
}
