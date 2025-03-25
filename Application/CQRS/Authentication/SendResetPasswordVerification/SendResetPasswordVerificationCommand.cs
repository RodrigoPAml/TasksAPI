using MediatR;
using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Authentication.SendResetPasswordVerification
{
    /// <summary>
    /// Command to send a verification code for the password reset
    /// </summary>
    public class SendResetPasswordVerificationCommand : IRequest<Operation>
    {
        [Required]
        public string Email { get; set; }
    }
}
