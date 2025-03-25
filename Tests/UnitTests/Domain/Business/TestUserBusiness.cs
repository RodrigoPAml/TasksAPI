using Moq;
using Domain.Enums;
using Domain.Entities;
using Domain.Business;
using Domain.Interfaces.Business;
using Domain.Interfaces.Repositories;
using Task = System.Threading.Tasks.Task;

namespace UnitTests.Domain.Business
{
    /// <summary>
    /// Unit tests for user business service
    /// </summary>
    public class TestUserBusiness
    {
        [Fact]
        public async Task TestNotAcceptNullCreate()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var business = new UserBusiness(serviceProvider.Object) as IUserBusiness;
            var result = await business.Create(null);

            Assert.False(result.Success);
        }

        [Theory]
        [InlineData("invalid", false)]
        [InlineData("email@email.com", true)]
        [InlineData("email2@email.com", false)]
        public async Task TestCreateBusiness(string name, bool expectedResult)
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var repoUser = new Mock<IUserRepository>();

            repoUser.Setup(repo => repo.ExistsByEmail("email@email.com"))
                .ReturnsAsync(false);
            repoUser.Setup(repo => repo.ExistsByEmail("email2@email.com"))
                .ReturnsAsync(true);

            serviceProvider.Setup(sp => sp.GetService(typeof(IUserRepository)))
                .Returns(repoUser.Object);

            var business = new UserBusiness(serviceProvider.Object) as IUserBusiness;

            var result = await business.Create(new()
            {
                Email = name,
                Password = "password12345",
                Username = "username",
            });

            Assert.Equal(result.Success, expectedResult);
        }

        [Fact]
        public void TestNotAcceptNullUpdatePassword()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var repoCategory = new Mock<ICategoryRepository>();

            var business = new UserBusiness(serviceProvider.Object) as IUserBusiness;

            var newEntityResult = User.Create(
                "email@email.com",
                "username",
                "password12345",
                string.Empty,
                ProfileTypeEnum.User
            );

            Assert.True(newEntityResult.Success);

            var result = business.UpdatePassword(new()
            {
                User = null,
                NewPassword = "newpassord12345"
            });

            Assert.False(result.Success);
        }

        [Fact]
        public void TestNotAcceptNullUpdateUsername()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var repoCategory = new Mock<ICategoryRepository>();

            var business = new UserBusiness(serviceProvider.Object) as IUserBusiness;

            var newEntityResult = User.Create(
                "email@email.com",
                "username",
                "password12345",
                string.Empty,
                ProfileTypeEnum.User
            );

            Assert.True(newEntityResult.Success);

            var result = business.UpdateUsername(new()
            {
                User = null,
                NewUsername = "username"
            });

            Assert.False(result.Success);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(1, false)]
        [InlineData(9, false)]
        [InlineData(10, true)]
        [InlineData(32, true)]
        [InlineData(33, false)]
        public void TestUpdatePasswordBusiness(int length, bool expectedResult)
        {
            var password = Utils.GenerateRandomString(length);

            var serviceProvider = new Mock<IServiceProvider>();
            var repoCategory = new Mock<ICategoryRepository>();

            var business = new UserBusiness(serviceProvider.Object) as IUserBusiness;

            var newEntityResult = User.Create(
                "email@email.com", 
                "username", 
                "password12345", 
                string.Empty,  
                ProfileTypeEnum.User
            );

            Assert.True(newEntityResult.Success);

            var entity = newEntityResult.Content;

            var result = business.UpdatePassword(new()
            {
                NewPassword = password,
                NewPasswordHashed = string.Empty,
                User = entity
            });

            Assert.Equal(result.Success, expectedResult);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(1, false)]
        [InlineData(3, false)]
        [InlineData(4, true)]
        [InlineData(64, true)]
        [InlineData(65, false)]
        public void TestUpdateUsernameBusiness(int length, bool expectedResult)
        {
            var username = Utils.GenerateRandomString(length);

            var serviceProvider = new Mock<IServiceProvider>();
            var repoCategory = new Mock<ICategoryRepository>();

            var business = new UserBusiness(serviceProvider.Object) as IUserBusiness;

            var newEntityResult = User.Create(
                "email@email.com",
                "username",
                "password12345",
                string.Empty,
                ProfileTypeEnum.User
            );

            Assert.True(newEntityResult.Success);

            var entity = newEntityResult.Content;

            var result = business.UpdateUsername(new()
            {
                NewUsername = username,
                User = entity
            });

            Assert.Equal(result.Success, expectedResult);
        }
    }
}
