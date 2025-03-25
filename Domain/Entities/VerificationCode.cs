using Domain.Base;
using Domain.Enums;
using Domain.Utils;

namespace Domain.Entities
{
    /// <summary>
    /// Verification code entity
    /// </summary>
    public sealed class VerificationCode : BaseEntity
    {
        public string Email { get; private set; }
        public DateTime Date { get; private set; }
        public int Code { get; private set; }
        public ConfirmationCodeTypeEnum Type { get; private set; }

        private VerificationCode() { }

        public static Result<VerificationCode> Create(
            string email,
            ConfirmationCodeTypeEnum type
        )
        {
            var code = new VerificationCode()
            {
                Email = email,
                Date = DateTime.UtcNow,
                Type = type,
                Code = new Random().Next(100000, 1000000),
            };

            var result = ValidateAll(code);

            if (!result.Success)
                return Result.MakeFailure<VerificationCode>(result.Message);

            return Result.MakeSuccess(code);
        }

        private static Operation ValidateAll(VerificationCode entity)
        {
            var emailValidation = entity.ValidateEmail();

            if (!emailValidation.Success)
                return emailValidation;

            var typeValidation = entity.ValidateType();

            if (!typeValidation.Success)
                return typeValidation;

            return Operation.MakeSuccess();
        }

        private Operation ValidateType()
        {
            return !Type.IsInRange()
                ? Operation.MakeFailure("Invalid type provided")
                : Operation.MakeSuccess();
        }

        private Operation ValidateEmail()
        {
            return EmailUtils.ValidateEmail(Email);
        }
    }
}