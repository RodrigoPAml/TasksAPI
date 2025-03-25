using Domain.Enums;
using Domain.Entities;
using IntegrationTests.Setup;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces.Transaction;
using Task = System.Threading.Tasks.Task;

namespace IntegrationTests.Tests.Infra
{
    /// <summary>
    /// Tests the user repository 
    /// </summary>
    [Collection("Integration tests")]
    public class UserRepositoryTests
    {
        private readonly WebApiSetup _setup;

        public UserRepositoryTests(WebApiSetup setup)
        {
            _setup = setup;
        }

        [Fact]
        public async Task TestGetById()
        {
            using (var scope = _setup.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IUserRepository>();
                var user = await repo.GetById(1);

                Assert.NotNull(user);
            }
        }

        [Fact]
        public async Task TestGetNullById()
        {
            using (var scope = _setup.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<IUserRepository>();
                var user = await repo.GetById(10);

                Assert.Null(user);
            }
        }

        [Fact]
        public async Task TestCreate()
        {
            using (var scope = _setup.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var repo = provider.GetService<IUserRepository>();

                var userResult = User.Create("email@test123.com", "testCreate", "password12345", string.Empty, ProfileTypeEnum.User);

                Assert.True(userResult.Success);

                var entity = userResult.Content;

                await provider.GetService<IUnityOfWork>().Begin();

                await repo.Create(entity);

                await provider.GetService<IUnityOfWork>().Save();
                await provider.GetService<IUnityOfWork>().Commit();

                Assert.True(entity.Id != default);
            }
        }

        [Fact]
        public async Task TestUpdateUsername()
        {
            using (var scope = _setup.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var repo = provider.GetService<IUserRepository>();
                var entity = await repo.GetById(1);

                Assert.NotNull(entity);

                await provider.GetService<IUnityOfWork>().Begin();

                await repo.UpdateUsername(entity.Id, "updatedUserName");

                await provider.GetService<IUnityOfWork>().Save();
                await provider.GetService<IUnityOfWork>().Commit();

                Assert.True(entity.Id != default);
            }
        }
    }
}