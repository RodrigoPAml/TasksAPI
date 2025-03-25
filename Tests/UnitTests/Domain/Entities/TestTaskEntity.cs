using Domain.Enums;
using Task = Domain.Entities.Task;

namespace UnitTests.Domain.Entities
{
    /// <summary>
    /// Unit tests for task entity
    /// </summary>
    public class TestTaskEntity
    {
        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(128, true)]
        [InlineData(129, false)]
        public void CreateNameValidation(int length, bool expectedSuccess)
        {
            var name = Utils.GenerateRandomString(length);

            var result = Task.Create(
                name,
                string.Empty,
                TaskStatusEnum.Pending,
                TaskPriorityEnum.Medium,
                DateTime.UtcNow,
                1,
                1
            );

            Assert.Equal(expectedSuccess, result.Success);
        }

        [Theory]
        [InlineData(-1, true)]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(100_000, true)]
        [InlineData(100_001, false)]
        public void CreateDescriptionValidation(int length, bool expectedSuccess)
        {
            var description = Utils.GenerateRandomString(length);

            var result = Task.Create(
                "ValidName",
                description,
                TaskStatusEnum.Pending,
                TaskPriorityEnum.Medium,
                DateTime.UtcNow,
                1,
                1
            );

            Assert.Equal(expectedSuccess, result.Success);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(4, true)]
        [InlineData(5, false)]
        public void CreateStatusValidation(int status, bool expectedSuccess)
        {
            var result = Task.Create(
                "ValidName",
                "ValidDescription",
                (TaskStatusEnum)status,
                TaskPriorityEnum.Medium,
                DateTime.UtcNow,
                1,
                1
            );

            Assert.Equal(expectedSuccess, result.Success);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(4, true)]
        [InlineData(5, false)]
        public void CreatePriorityValidation(int priority, bool expectedSuccess)
        {
            var result = Task.Create(
                "ValidName",
                "ValidDescription",
                TaskStatusEnum.Pending,
                (TaskPriorityEnum)priority,
                DateTime.UtcNow,
                1,
                1
            );

            Assert.Equal(expectedSuccess, result.Success);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        public void CreateUserIdValidation(int userId, bool expectedSuccess)
        {
            var result = Task.Create(
                "ValidName",
                "ValidDescription",
                TaskStatusEnum.Pending,
                TaskPriorityEnum.Medium,
                DateTime.UtcNow,
                userId,
                1
            );

            Assert.Equal(expectedSuccess, result.Success);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        public void CreateCategoryIdValidation(int categoryId, bool expectedSuccess)
        {
            var result = Task.Create(
                "ValidName",
                "ValidDescription",
                TaskStatusEnum.Pending,
                TaskPriorityEnum.Medium,
                DateTime.UtcNow,
                1,
                categoryId
            );

            Assert.Equal(expectedSuccess, result.Success);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(128, true)]
        [InlineData(129, false)]
        public void UpdateNameValidation(int length, bool expectedSuccess)
        {
            var name = Utils.GenerateRandomString(length);

            var newEntityResult = Task.Create(
                "ValidName",
                string.Empty,
                TaskStatusEnum.Pending,
                TaskPriorityEnum.Medium,
                DateTime.UtcNow,
                1,
                1
            );

            Assert.True(newEntityResult.Success);

            var result = newEntityResult.Content.Update(
                name,
                string.Empty,
                TaskStatusEnum.Pending,
                TaskPriorityEnum.Medium,
                DateTime.UtcNow,
                1
            );

            Assert.Equal(expectedSuccess, result.Success);
        }

        [Theory]
        [InlineData(-1, true)]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(100_000, true)]
        [InlineData(100_001, false)]
        public void UpdateDescriptionValidation(int length, bool expectedSuccess)
        {
            var description = Utils.GenerateRandomString(length);

            var newEntityResult = Task.Create(
                "ValidName",
                string.Empty,
                TaskStatusEnum.Pending,
                TaskPriorityEnum.Medium,
                DateTime.UtcNow,
                1,
                1
            );

            Assert.True(newEntityResult.Success);

            var result = newEntityResult.Content.Update(
                "ValidName",
                description,
                TaskStatusEnum.Pending,
                TaskPriorityEnum.Medium,
                DateTime.UtcNow,
                1
            );

            Assert.Equal(expectedSuccess, result.Success);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(4, true)]
        [InlineData(5, false)]
        public void UpdateStatusValidation(int status, bool expectedSuccess)
        {
            var newEntityResult = Task.Create(
                "ValidName",
                "ValidDescription",
                TaskStatusEnum.Paused,
                TaskPriorityEnum.Medium,
                DateTime.UtcNow,
                1,
                1
            );

            Assert.True(newEntityResult.Success);

            var result = newEntityResult.Content.Update(
               "ValidName",
               "ValidDescription",
               (TaskStatusEnum)status,
               TaskPriorityEnum.Medium,
               DateTime.UtcNow,
               1
           );

            Assert.Equal(expectedSuccess, result.Success);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(4, true)]
        [InlineData(5, false)]
        public void UpdatePriorityValidation(int priority, bool expectedSuccess)
        {
            var newEntityResult = Task.Create(
                "ValidName",
                "ValidDescription",
                TaskStatusEnum.Pending,
                TaskPriorityEnum.None,
                DateTime.UtcNow,
                1,
                1
            );

            Assert.True(newEntityResult.Success);

            var result = newEntityResult.Content.Update(
                "ValidName",
                "ValidDescription",
                TaskStatusEnum.Pending,
                (TaskPriorityEnum)priority,
                DateTime.UtcNow,
                1
            );

            Assert.Equal(expectedSuccess, result.Success);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        public void UpdateCategoryIdValidation(int categoryId, bool expectedSuccess)
        {
            var newEntityResult = Task.Create(
                "ValidName",
                "ValidDescription",
                TaskStatusEnum.Pending,
                TaskPriorityEnum.Medium,
                DateTime.UtcNow,
                1,
                1
            );

            Assert.True(newEntityResult.Success);

            var result = newEntityResult.Content.Update(
                "ValidName",
                "ValidDescription",
                TaskStatusEnum.Pending,
                TaskPriorityEnum.High,
                DateTime.UtcNow,
                categoryId
            );

            Assert.Equal(expectedSuccess, result.Success);
        }
    }
}