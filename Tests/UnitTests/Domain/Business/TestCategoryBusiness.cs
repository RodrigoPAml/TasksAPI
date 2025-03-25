using Moq;
using Domain.Entities;
using Domain.Business;
using Domain.Models.Categories;
using Domain.Interfaces.Business;
using Domain.Interfaces.Repositories;
using Task = System.Threading.Tasks.Task;

namespace UnitTests.Domain.Business
{
    /// <summary>
    /// Unit tests for category business service
    /// </summary>
    public class TestCategoryBusiness
    {
        [Fact]
        public async Task TestNotAcceptNullCreate()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var business = new CategoryBusiness(serviceProvider.Object) as ICategoryBusiness;
            var result = await business.Create(null);

            Assert.False(result.Success);
        }

        [Theory]
        [InlineData("UsedName", 1, false)]
        [InlineData("NotUsedName", 1, true)]
        [InlineData("NotUsedName", 2, false)]
        [InlineData("", 1, false)]
        public async Task TestCreateBusiness(string name, int userId, bool expectedResult)
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var repoCategory = new Mock<ICategoryRepository>();
            var repoUser = new Mock<IUserRepository>();

            repoCategory.Setup(repo => repo.ExistsByName("UsedName", 1, null))
                .ReturnsAsync(true);
            repoCategory.Setup(repo => repo.ExistsByName("NotUsedName", 1, null))
                .ReturnsAsync(false);
            repoCategory.Setup(repo => repo.ExistsByName("NotUsedName", 2, null))
                .ReturnsAsync(false);

            repoUser.Setup(repo => repo.ExistsById(1))
                .ReturnsAsync(true);
            repoUser.Setup(repo => repo.ExistsById(2))
                .ReturnsAsync(false);

            serviceProvider.Setup(sp => sp.GetService(typeof(ICategoryRepository)))
                .Returns(repoCategory.Object);
            serviceProvider.Setup(sp => sp.GetService(typeof(IUserRepository)))
                .Returns(repoUser.Object);

            var business = new CategoryBusiness(serviceProvider.Object) as ICategoryBusiness;

            var result = await business.Create(new()
            {
                Name = name,
                UserId = userId
            });

            Assert.Equal(result.Success, expectedResult);
        }

        [Fact]
        public async Task TestNotAcceptNullUpdate()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var business = new CategoryBusiness(serviceProvider.Object) as ICategoryBusiness;
            var result = await business.Update(null);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task TestNotAcceptNullInfoUpdate()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var business = new CategoryBusiness(serviceProvider.Object) as ICategoryBusiness;
            var result = await business.Update(new()
            {
                Info = null,
                Category = Category.Create("Category", 1).Content
            });

            Assert.False(result.Success);
        }

        [Fact]
        public async Task TestNotAcceptNullCategoryUpdate()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var business = new CategoryBusiness(serviceProvider.Object) as ICategoryBusiness;
            var result = await business.Update(new()
            {
                Category = null,
                Info = new(),
            });

            Assert.False(result.Success);
        }

        [Theory]
        [InlineData(1, "UsedName", 1, false)]
        [InlineData(1, "NotUsedName", 1, true)]
        [InlineData(1, "NotUsedName", 2, false)]
        [InlineData(2, "NotUsedName", 2, true)]
        public async Task TestUpdateBusiness(int id, string name, int userId, bool expectedResult)
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var repoCategory = new Mock<ICategoryRepository>();

            repoCategory.Setup(repo => repo.ExistsByName("UsedName", 1, 1))
                .ReturnsAsync(true);
            repoCategory.Setup(repo => repo.ExistsByName("NotUsedName", 1, 1))
                .ReturnsAsync(false);
            repoCategory.Setup(repo => repo.ExistsByName("NotUsedName", 2, 1))
                .ReturnsAsync(true);
            repoCategory.Setup(repo => repo.ExistsByName("NotUsedName", 2, 2))
                .ReturnsAsync(false);

            serviceProvider.Setup(sp => sp.GetService(typeof(ICategoryRepository)))
                .Returns(repoCategory.Object);

            var entityResult = Category.Create("Category", userId);
            Assert.True(entityResult.Success);

            var entity = entityResult.Content;
            Utils.InjectValue(entity, nameof(entity.Id), id); // Set private id for testing purposes

            var business = new CategoryBusiness(serviceProvider.Object) as ICategoryBusiness;

            var result = await business.Update(new()
            {
                Category = entity,
                Info = new UpdateCategoryInfoModel()
                {
                    Name = name
                }
            });

            Assert.Equal(result.Success, expectedResult);
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, true)]
        public async Task TesDeleteBusiness(int id, bool expectedResult)
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var repoCategory = new Mock<ICategoryRepository>();

            repoCategory.Setup(repo => repo.IsUsedInTasks(1))
                .ReturnsAsync(true);
            repoCategory.Setup(repo => repo.IsUsedInTasks(2))
              .ReturnsAsync(false);

            serviceProvider.Setup(sp => sp.GetService(typeof(ICategoryRepository)))
                .Returns(repoCategory.Object);

            var entityResult = Category.Create("Category", 1);
            Assert.True(entityResult.Success);

            var entity = entityResult.Content;
            Utils.InjectValue(entity, nameof(entity.Id), id); // Set private id for testing purposes

            var business = new CategoryBusiness(serviceProvider.Object) as ICategoryBusiness;

            var result = await business.CanDelete(new()
            {
                Category = entity,
            });

            Assert.Equal(result.Success, expectedResult);
        }

        [Fact]
        public async Task TestNotAcceptNullDelete()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var business = new CategoryBusiness(serviceProvider.Object) as ICategoryBusiness;
            var result = await business.CanDelete(null);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task TestNotAcceptNullCategoryDelete()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var business = new CategoryBusiness(serviceProvider.Object) as ICategoryBusiness;
            var result = await business.CanDelete(new()
            {
                Category = null,
            });

            Assert.False(result.Success);
        }
    }
}
