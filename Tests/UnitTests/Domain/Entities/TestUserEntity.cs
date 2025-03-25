using Domain.Entities;
using Domain.Enums;

namespace UnitTests.Domain.Entities
{
    /// <summary>
    /// Unit tests for user entity
    /// </summary>
    public class TestUserEntity
    {
        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("invalidemail", false)]
        [InlineData("invalid@email", false)]
        [InlineData("invalid@.com", false)]
        [InlineData("@missingusername.com", false)]
        [InlineData("email@email.com", true)]
        [InlineData("user.name+tag+sorting@example.com", true)]
        [InlineData("x@example.com", true)]
        [InlineData("example-indeed@strange-example.com", true)]
        [InlineData("test.email.with+symbol@example.com", true)]
        public void CreateEmailValidation(string email, bool expectedSuccess)
        {
            var result = User.Create(
                email,
                "User",
                "Password12345",
                string.Empty,
                ProfileTypeEnum.User
            );

            Assert.Equal(expectedSuccess, result.Success);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(1, false)]
        [InlineData(3, false)]
        [InlineData(4, true)]
        [InlineData(64, true)]
        [InlineData(65, false)]
        public void CreateUsernameValidation(int length, bool expectedSuccess)
        {
            string username = Utils.GenerateRandomString(length);

            var result = User.Create(
                "email@email.com",
                username,
                "Password12345",
                string.Empty,
                ProfileTypeEnum.User
            );

            Assert.Equal(expectedSuccess, result.Success);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(9, false)]
        [InlineData(10, true)]
        [InlineData(31, true)]
        [InlineData(32, true)]
        [InlineData(33, false)]
        public void CreatePasswordValidation(int length, bool expectedSuccess)
        {
            string password = Utils.GenerateRandomString(length);

            var result = User.Create(
                "email@email.com",
                "rodrigo",
                password,
                string.Empty,
                ProfileTypeEnum.User
            );

            Assert.Equal(expectedSuccess, result.Success);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(2, false)]
        public void CreateProfileValidation(int value, bool expectedSuccess)
        {
            var profile = (ProfileTypeEnum)value;

            var result = User.Create(
                "email@email.com",
                "rodrigo",
                "1234512345",
                string.Empty,
                profile
            );

            Assert.Equal(expectedSuccess, result.Success);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(1, false)]
        [InlineData(3, false)]
        [InlineData(4, true)]
        [InlineData(64, true)]
        [InlineData(65, false)]
        public void ChangeUsernameValidation(int length, bool expectedSuccess)
        {
            string username = Utils.GenerateRandomString(length);

            var result = User.Create(
                "email@email.com",
                "teste",
                "Password12345",
                string.Empty,
                ProfileTypeEnum.User
            );

            var changeResult = result.Content.ChangeUsername(username);

            Assert.Equal(expectedSuccess, changeResult.Success);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(9, false)]
        [InlineData(10, true)]
        [InlineData(31, true)]
        [InlineData(32, true)]
        [InlineData(33, false)]
        public void ChangePasswordValidation(int length, bool expectedSuccess)
        {
            string password = Utils.GenerateRandomString(length);

            var result = User.Create(
                "email@email.com",
                "rodrigo",
                "Password12345",
                string.Empty,
                ProfileTypeEnum.User
            );

            var changeResult = result.Content.ChangePassword(password, string.Empty);

            Assert.Equal(expectedSuccess, changeResult.Success);
        }
    }
}
