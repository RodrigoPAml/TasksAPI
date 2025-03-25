using Domain.Entities;

namespace UnitTests.Domain.Entities
{
    /// <summary>
    /// Unit tests for category entity
    /// </summary>
    public class TestCategoryEntity
    {
        [Theory]
        [InlineData(-1, 1, false)]
        [InlineData(0, 1, false)]
        [InlineData(1, 1, true)]
        [InlineData(1, -1, false)]
        [InlineData(1, 0, false)]
        [InlineData(128, 1, true)]
        [InlineData(129, 1, false)]
        public void CreateCategory(int length, int userId, bool expectedSuccess)
        {
            var name = Utils.GenerateRandomString(length);

            var result = Category.Create(name, userId);
            Assert.Equal(expectedSuccess, result.Success);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(128, true)]
        [InlineData(129, false)]
        public void ChangeCategoryName(int length, bool expectedSuccess)
        {
            var result = Category.Create("test", 1);

            Assert.True(result.Success);

            var name = Utils.GenerateRandomString(length);
            var changeResult = result.Content.ChangeName(name);

            Assert.Equal(expectedSuccess, changeResult.Success);
        }
    }
}