using Domain.Entities;
using Domain.Enums;

namespace UnitTests.Domain.Entities
{
    /// <summary>
    /// Unit tests for verification code entity
    /// </summary>
    public class TestVerificationCodeEntity
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
            var result = VerificationCode.Create(
                email,
                ConfirmationCodeTypeEnum.Email
            );

            Assert.Equal(expectedSuccess, result.Success);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(2, false)]
        public void CreateTypeValidation(int type, bool expectedSuccess)
        {
            var result = VerificationCode.Create(
                "email@email.com",
                (ConfirmationCodeTypeEnum)type
            );

            Assert.Equal(expectedSuccess, result.Success);
        }
    }
}