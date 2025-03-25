using Moq;
using Domain.Enums;
using Domain.Business;
using Domain.Interfaces.Business;
using Domain.Interfaces.Repositories;
using TaskEntity = Domain.Entities.Task;
using Task = System.Threading.Tasks.Task;

namespace UnitTests.Domain.Business
{
    /// <summary>
    /// Unit tests for task business service
    /// </summary>
    public class TestTaskBusiness
    {
        [Fact]
        public async Task TestNotAcceptNullCreate()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var business = new TaskBusiness(serviceProvider.Object) as ITaskBusiness;
            var result = await business.Create(null);

            Assert.False(result.Success);
        }

        [Theory]
        [InlineData("", 1, 1, false)]
        [InlineData("NotUsedName", 1, 1, true)]
        [InlineData("NotUsedName", 1, 2, false)]
        [InlineData("NotUsedName", 2, 1, false)]
        [InlineData("UsedName", 1, 1, false)]
        public async Task TestCreateBusiness(string name, int userId, int categoryId, bool expectedResult)
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var userRepo = new Mock<IUserRepository>();
            var categoryRepo = new Mock<ICategoryRepository>();
            var taskRepo = new Mock<ITaskRepository>();

            userRepo.Setup(repo => repo.ExistsById(1))
                .ReturnsAsync(true);
            userRepo.Setup(repo => repo.ExistsById(2))
                .ReturnsAsync(false);

            categoryRepo.Setup(repo => repo.ExistsById(1, 1))
                .ReturnsAsync(true);
            categoryRepo.Setup(repo => repo.ExistsById(2, 1))
                .ReturnsAsync(false);

            taskRepo.Setup(repo => repo.ExistsByName("UsedName", 1, null))
                .ReturnsAsync(true);
            taskRepo.Setup(repo => repo.ExistsByName("NotUsedName", 1, null))
                .ReturnsAsync(false);

            serviceProvider.Setup(sp => sp.GetService(typeof(ITaskRepository)))
                .Returns(taskRepo.Object);

            serviceProvider.Setup(sp => sp.GetService(typeof(IUserRepository)))
                .Returns(userRepo.Object);

            serviceProvider.Setup(sp => sp.GetService(typeof(ICategoryRepository)))
                .Returns(categoryRepo.Object);

            var business = new TaskBusiness(serviceProvider.Object) as ITaskBusiness;

            var result = await business.Create(new()
            {
                Name = name,
                Description = string.Empty,
                UserId = userId,
                CategoryId = categoryId,
                DueDate = DateTime.Now,
                Priority = TaskPriorityEnum.Medium,
                Status = TaskStatusEnum.Pending
            });

            Assert.Equal(result.Success, expectedResult);
        }

        [Theory]
        [InlineData("", 1, false)]
        [InlineData("NotUsedName", 1, true)]
        [InlineData("NotUsedName", 2, false)]
        [InlineData("UsedName", 1, false)]
        public async Task TestUpdateBusiness(string name, int categoryId, bool expectedResult)
        {
            var taskCreateResult = TaskEntity.Create(
                "TaskName",
                "TaskDescription",
                TaskStatusEnum.Pending,
                TaskPriorityEnum.Low,
                DateTime.UtcNow,
                1,
                1
            );

            Utils.InjectValue(taskCreateResult.Content, nameof(taskCreateResult.Content.Id), 1); // simulate id as 1
            Assert.True(taskCreateResult.Success);

            var serviceProvider = new Mock<IServiceProvider>();
            var categoryRepo = new Mock<ICategoryRepository>();
            var taskRepo = new Mock<ITaskRepository>();

            categoryRepo.Setup(repo => repo.ExistsById(1, 1))
                .ReturnsAsync(true);
            categoryRepo.Setup(repo => repo.ExistsById(2, 1))
                .ReturnsAsync(false);

            taskRepo.Setup(repo => repo.ExistsByName("UsedName", 1, 1))
                .ReturnsAsync(true);
            taskRepo.Setup(repo => repo.ExistsByName("NotUsedName", 1, 1))
                .ReturnsAsync(false);

            serviceProvider.Setup(sp => sp.GetService(typeof(ITaskRepository)))
                .Returns(taskRepo.Object);
            serviceProvider.Setup(sp => sp.GetService(typeof(ICategoryRepository)))
                .Returns(categoryRepo.Object);

            var business = new TaskBusiness(serviceProvider.Object) as ITaskBusiness;

            var result = await business.Update(new()
            {
                Task = taskCreateResult.Content,
                Info = new()
                {
                    Name = name,
                    Description = string.Empty,
                    CategoryId = categoryId,
                    DueDate = DateTime.Now,
                    Priority = TaskPriorityEnum.Medium,
                    Status = TaskStatusEnum.Pending,
                }
            });

            Assert.Equal(result.Success, expectedResult);
        }
    }
}
