using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.Entities;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace xUnitTests
{
    public class UserTests
    {
        [Fact]
        public void Authenticate_ShouldReturnNull_WhenUserDoesNotExist()
        {
            IUserRepository fakeRepo = A.Fake<IUserRepository>();
            UserService service = new UserService(fakeRepo);

            A.CallTo(() => fakeRepo.GetByUsername("spongebob")).Returns(null);

            var result = service.Authenticate("spongebob", "password");

            Assert.Null(result);
        }

        [Theory]
        [InlineData(null, "password")]
        [InlineData("", "password")]
        [InlineData("   ", "password")]
        [InlineData("username", null)]
        [InlineData("username", "")]
        [InlineData("username", "   ")]
        public void Authenticate_ShouldReturnNull_WhenUsernameOrPasswordIsNullOrWhitespace(string username, string password)
        {
            IUserRepository fakeRepo = A.Fake<IUserRepository>();
            UserService service = new UserService(fakeRepo);

            var result = service.Authenticate(username, password);

            Assert.Null(result);
            A.CallTo(() => fakeRepo.GetByUsername(A<string>._)).MustNotHaveHappened();
        }

        [Fact]
        public void Authenticate_ShouldReturnUser_WhenHashedPasswordMatches()
        {
            IUserRepository fakeRepo = A.Fake<IUserRepository>();
            UserService service = new UserService(fakeRepo);

            var hashed = PasswordHasher.HashPassword("correctpassword");
            var user = new User{ 
                Id = 1,
                Username = "alice",
                Password = hashed
            };

            A.CallTo(() => fakeRepo.GetByUsername("alice")).Returns(user);

            var result = service.Authenticate("alice", "correctpassword");

            Assert.NotNull(result);
            Assert.Equal(user, result);
        }

        [Fact]
        public void Authenticate_ShouldReturnNull_WhenHashedPasswordDoesNotMatch()
        {
            IUserRepository fakeRepo = A.Fake<IUserRepository>();
            UserService service = new UserService(fakeRepo);

            var hashed = PasswordHasher.HashPassword("correctpassword");
            var user = new User {
                Id = 1,
                Username = "alice",
                Password = hashed
            };

            A.CallTo(() => fakeRepo.GetByUsername("alice")).Returns(user);

            var result = service.Authenticate("alice", "wrongpassword");

            Assert.Null(result);
        }

        [Fact]
        public void Authenticate_ShouldReturnUser_AndUpgradePasswordHash_WhenPlainTextPasswordMatches()
        {
            IUserRepository fakeRepo = A.Fake<IUserRepository>();
            UserService service = new UserService(fakeRepo);

            var user = new User {
                Id = 1,
                Username = "spongebob",
                Password = "plaintext"
            };

            A.CallTo(() => fakeRepo.GetByUsername("spongebob")).Returns(user);

            var result = service.Authenticate("spongebob", "plaintext");

            Assert.NotNull(result);
            Assert.True(PasswordHasher.IsHashFormat(result.Password));

            A.CallTo(() => fakeRepo.UpdatePasswordHash(user.Id, A<string>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void Authenticate_ShouldReturnNull_WhenPlainTextPasswordDoesNotMatch()
        {
            IUserRepository fakeRepo = A.Fake<IUserRepository>();
            UserService service = new UserService(fakeRepo);

            var user = new User { Id = 1, Username = "spongebob", Password = "plaintext" };
            A.CallTo(() => fakeRepo.GetByUsername("spongebob")).Returns(user);

            var result = service.Authenticate("spongebob", "wrongpassword");

            Assert.Null(result);
            A.CallTo(() => fakeRepo.UpdatePasswordHash(A<int>._, A<string>._)).MustNotHaveHappened();
        }
    }
}
